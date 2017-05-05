using Cheesebaron.MvxPlugins.DeviceInfo;
using Conscience.Domain;
using Conscience.Plugins;
using Microsoft.AspNet.SignalR.Client;
using MvvmCross.Plugins.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Conscience.Mobile.Hosts.Core.Services
{
    public class HostsService
    {
        private readonly AppState _appState;
        private readonly IMvxLocationWatcher _locationWatcher;
        private readonly IAudioService _audioService;
        private readonly IBatteryService _batteryService;
        private readonly IDeviceInfo _deviceInfo;
        private Timer _hubTimer;
        private HubConnection _hubConn;
        private IHubProxy _hostsHub;

        private object _locationLock = new object();
        private List<Location> LocationsBuffer = new List<Location>();
        private string _locationError;

        public event EventHandler<HostsEventArgs<Location>> LocationUpdated;

        public HostsService(AppState appState, IMvxLocationWatcher locationWatcher, IBatteryService batteryService, IAudioService audioService, IDeviceInfo deviceInfo)
        {
            _appState = appState;
            _locationWatcher = locationWatcher;
            _batteryService = batteryService;
            _audioService = audioService;
            _deviceInfo = deviceInfo;
        }

        public async void Start()
        {
            _locationWatcher.Start(new MvxLocationOptions() { Accuracy = MvxLocationAccuracy.Fine, TrackingMode = MvxLocationTrackingMode.Background }, GeoLocationSuccess, GeoLocationFailure);
            
            _hubTimer = new Timer(HubTimerTick, null, TimeSpan.FromMinutes(0), TimeSpan.FromSeconds(10));

            _hubConn = new HubConnection(_appState.ServerUrl + "/signalr/hubs");
            _hubConn.CookieContainer = _appState.CookieContainer;
            _hostsHub = _hubConn.CreateHubProxy("HostsHub");

            await _hubConn.Start();

            _hostsHub.On<NotificationAudio>("NotificationAudio", HandleNotificationSound);

            await _hostsHub.Invoke("SubscribeHost", _deviceInfo.DeviceId);
        }

        public void Stop()
        {
            _hubConn.Dispose();
        }


        private void GeoLocationSuccess(MvxGeoLocation geo)
        {
            _locationError = string.Empty;

            Location location = null;

            lock (_locationLock)
            {
                location = new Location
                {
                    Latitude = geo.Coordinates.Latitude,
                    Longitude = geo.Coordinates.Longitude,
                    Accuracy = geo.Coordinates.Accuracy,
                    Altitude = geo.Coordinates.Altitude,
                    AltitudeAccuracy = geo.Coordinates.AltitudeAccuracy,
                    Heading = geo.Coordinates.Heading,
                    HeadingAccuracy = geo.Coordinates.HeadingAccuracy,
                    Speed = geo.Coordinates.Speed,
                    TimeStamp = DateTime.Now
                };
                LocationsBuffer.Add(location);
            }

            LocationUpdated?.Invoke(this, new HostsEventArgs<Location>(location));
        }

        private void GeoLocationFailure(MvxLocationError error)
        {
            _locationError = error.Code.ToString();

            LocationUpdated?.Invoke(this, new HostsEventArgs<Location>(null) { Error = _locationError });
        }
        
        private void PlayRandomNumber()
        {
            var random = new Random().Next(10) + 1;

            var path = "Conscience.Mobile.Hosts.Core.Audio.Numbers." + random + ".mp3";

            var assembly = GetType().GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream(path);

            _audioService.PlaySound(stream);
        }
        
        private void HubTimerTick(object status)
        {
            lock (_locationLock)
            {
                if (LocationsBuffer.Any() && _hubConn.State == ConnectionState.Connected)
                {
                    _hostsHub.Invoke("LocationUpdates", LocationsBuffer.ToList());

                    LocationsBuffer.Clear();

                    //TODO: Send battery level
                    //BatteryLevel = _batteryService.RemainingChargePercent;
                    //BatteryStatus = _batteryService.Status.ToString();
                }
            }
        }

        public void HandleNotificationSound(NotificationAudio notification)
        {
            PlayRandomNumber();
        }
    }

    public class HostsEventArgs<T> : EventArgs
    {
        private T _data;

        public HostsEventArgs(T data)
        {
            _data = data;
        }

        public T Data
        {
            get
            {
                return _data;
            }
        }

        public string Error
        {
            get;
            set;
        }
    }
}
