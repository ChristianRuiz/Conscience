using Conscience.Plugins;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Conscience.Mobile.Hosts.Core.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        IMvxLocationWatcher _locationWatcher;
        IBatteryService _batteryService;
        Timer _batteryTimer;

        private double _latitude;
        public double Latitude
        {
            get { return _latitude; }
            set
            {
                _latitude = value;
                RaisePropertyChanged(() => Latitude);
            }
        }

        private double _longitude;
        public double Longitude
        {
            get { return _longitude; }
            set
            {
                _longitude = value;
                RaisePropertyChanged(() => Longitude);
            }
        }

        private double? _accuracy;
        public double? Accuracy
        {
            get { return _accuracy; }
            set
            {
                _accuracy = value;
                RaisePropertyChanged(() => Accuracy);
            }
        }

        private string _error;
        public string Error
        {
            get { return _error; }
            set
            {
                _error = value;
                RaisePropertyChanged(() => Error);
            }
        }

        private int _batteryLevel;
        public int BatteryLevel
        {
            get { return _batteryLevel; }
            set
            {
                _batteryLevel = value;
                RaisePropertyChanged(() => BatteryLevel);
            }
        }

        private string _batteryStatus;
        public string BatteryStatus
        {
            get { return _batteryStatus; }
            set
            {
                _batteryStatus = value;
                RaisePropertyChanged(() => BatteryStatus);
            }
        }


        public MainViewModel(IMvxLocationWatcher locationWatcher, IBatteryService batteryService)
        {
            _locationWatcher = locationWatcher;
            _batteryService = batteryService;
        }
        
        public override void Start()
        {
            _locationWatcher.Start(new MvxLocationOptions() { Accuracy = MvxLocationAccuracy.Fine }, GeoLocationSuccess, GeoLocationFailure);

            _batteryTimer = new Timer(BatteryServiceTimerTick, null, TimeSpan.FromMinutes(0), TimeSpan.FromSeconds(5));
        }

        private void GeoLocationSuccess(MvxGeoLocation geo)
        {
            Error = string.Empty;
            Latitude = geo.Coordinates.Latitude;
            Longitude = geo.Coordinates.Longitude;
            Accuracy = geo.Coordinates.Accuracy;
        }

        private void GeoLocationFailure(MvxLocationError error)
        {
            Error = error.Code.ToString();
        }

        private void BatteryServiceTimerTick(object status)
        {
            BatteryLevel = _batteryService.RemainingChargePercent;
            BatteryStatus = _batteryService.Status.ToString();
        }
    }
}
