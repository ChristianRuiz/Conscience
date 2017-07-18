import { Platform } from 'react-native';
import BackgroundTimer from 'react-native-background-timer';
import DeviceBattery from 'react-native-device-battery';
import DeviceInfo from 'react-native-device-info';
import Location from 'react-native-location';
import reportException from './ReportException';
import updateCache from './CacheService';

import Constants from '../constants';

class SignalRService {
  constructor(apolloClient, navigator, audioService) {
    this.client = apolloClient;
    this.audioService = audioService;

    this._onTimer = this._onTimer.bind(this);

    if (Platform.OS === 'ios') {
      Location.requestAlwaysAuthorization();
    }

    this.deviceId = DeviceInfo.getDeviceId();

    this.batteryLevel = -1;
    this.charging = false;

    const onBatteryStateChanged = (state) => {
      this.batteryLevel = state.level;
      this.charging = state.charging;
    };

    DeviceBattery.addListener(onBatteryStateChanged);

    DeviceBattery.getBatteryLevel().then((batteryLevel) => {
      this.batteryLevel = batteryLevel;
    });
    DeviceBattery.isCharging().then((charging) => {
      this.charging = charging;
    });

    this.locations = [];

    this.watchID = navigator.geolocation.watchPosition((location) => {
      this.locations.push(location);
    }, (error) => {
      throw error;
    },
      {
        enableHighAccuracy: true,
        timeout: 10000,
        maximumAge: 0
      });

    if (this.intervalId) {
      BackgroundTimer.clearInterval(this.intervalId);
    }

    this.intervalId = BackgroundTimer.setInterval(() => {
      this._onTimer();
    }, 1000 * 15);

    this._onTimer();
  }

  _onTimer() {
    // Hack: playing a sound on every timer tick to avoid Android OS to shut us down
    this.audioService.playSound('empty.mp3');

    // this.audioService.playSound('1.mp3');

    const update = {
      deviceId: this.deviceId,
      locations: []
    };

    Object.assign(update, {
      batteryLevel: this.batteryLevel,
      charging: this.charging
    });

    const serviceLocations = this.locations.map(l => ({
      Latitude: l.coords.latitude,
      Longitude: l.coords.longitude,
      Accuracy: l.coords.accuracy,
      Altitude: l.coords.altitude,
      Heading: l.coords.heading,
      Speed: l.coords.speed,
      TimeStamp: new Date(l.timestamp).toISOString()
    }));

    if (this.locations.length > 0) {
      Object.assign(update, {
        locations: serviceLocations
      });
    }

    try {
      fetch(`${Constants.SERVER_URL}/api/Notifications`, {
        method: 'POST',
        body: JSON.stringify(update)
      }).then(() => {
        this.locations = [];
      })
      .catch((e) => { console.log(`Unable to to send location updates to the server (ajax): ${e}`); });
    } catch (e) {
      console.log(`Unable to send updates to the server: ${e}`);
      reportException(`Unable to send updates to the server: ${e}`);
    }

    updateCache(this.client, (data) => {
      const device = data.accounts.current.host.account.device;

      device.deviceId = this.deviceId;
      device.batteryLevel = this.batteryLevel;
      device.batteryStatus = this.charging ? 'CHARGING' : 'NOT_CHARGING';

      if (device.currentLocation && serviceLocations.length > 0) {
        const currentLocation = serviceLocations[serviceLocations.length - 1];

        device.currentLocation.latitude = currentLocation.Latitude;
        device.currentLocation.longitude = currentLocation.Longitude;
        device.currentLocation.timeStamp = currentLocation.TimeStamp;
      }
    });
  }
}

export default SignalRService;
