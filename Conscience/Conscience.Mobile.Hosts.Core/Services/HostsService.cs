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
#if DEBUG
        private const string ServerUrl = "http://192.168.1.42/";
#else
        private const string ServerUrl = "http://consciencelarp.azurewebsites.net/";
#endif
        IMvxLocationWatcher _locationWatcher;
        IAudioService _audioService;
        IBatteryService _batteryService;
        Timer _hubTimer;
        HubConnection _hubConn;
        IHubProxy _hostsHub;

        private object _locationLock = new object();
        private List<Location> LocationsBuffer = new List<Location>();
        private string _locationError;

        public event EventHandler<HostsEventArgs<Location>> LocationUpdated;

        public HostsService(IMvxLocationWatcher locationWatcher, IBatteryService batteryService, IAudioService audioService)
        {
            _locationWatcher = locationWatcher;
            _batteryService = batteryService;
            _audioService = audioService;
        }

        public async void Start()
        {
            _locationWatcher.Start(new MvxLocationOptions() { Accuracy = MvxLocationAccuracy.Fine, TrackingMode = MvxLocationTrackingMode.Background }, GeoLocationSuccess, GeoLocationFailure);
            
            _hubTimer = new Timer(HubTimerTick, null, TimeSpan.FromMinutes(0), TimeSpan.FromSeconds(10));

            _hubConn = new HubConnection(ServerUrl + "/signalr/hubs");
            _hostsHub = _hubConn.CreateHubProxy("HostsHub");

            await _hubConn.Start();

            _hostsHub.On<NotificationAudio>("NotificationAudio", HandleNotificationSound);

            await _hostsHub.Invoke("SubscribeHost");
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
                if (LocationsBuffer.Any())
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
