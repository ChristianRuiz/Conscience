import React from 'react';
import { View } from 'react-native';
import DeviceBattery from 'react-native-device-battery';
import DeviceInfo from 'react-native-device-info';

class SignalRService extends React.Component {
  constructor(props) {
    super(props);

    this._onTimer = this._onTimer.bind(this);

    this.deviceId = DeviceInfo.getDeviceId();

    this.batteryLevel = -1;
    this.charging = false;

    const onBatteryStateChanged = (state) => {
      const initialized = this.batteryLevel;

      this.batteryLevel = state.level;
      this.charging = state.charging;

      if (!initialized) {
        this._onTimer();
      }
    };

    DeviceBattery.addListener(onBatteryStateChanged);

    DeviceBattery.getBatteryLevel().then((batteryLevel) => {
      this.batteryLevel = batteryLevel;
    });
    DeviceBattery.isCharging().then((charging) => {
      this.charging = charging;
    });

    this.locations = [];
    this.locationsInitilized = false;

    this.watchID = navigator.geolocation.watchPosition((location) => {
      this.locations.push(location);

      if (!this.locationsInitilized) {
        this.locationsInitilized = true;
        this._onTimer();
      }
    });

    setInterval(this._onTimer, 1000 * 15);
  }

  _onTimer() {
    if (this.props.listener) {
      const update = {
        deviceId: this.deviceId
      };

      Object.assign(update, {
        batteryLevel: this.batteryLevel,
        charging: this.charging
      });

      if (this.locations.length > 0) {
        // TODO: Consolidate locations
        Object.assign(update, {
          locations: this.locations
        });

        this.locations = [];
      }

      this.props.listener(update);
    }
  }

  render() {
    return <View />;
  }
}

SignalRService.propTypes = {
  listener: React.PropTypes.func
};

SignalRService.defaultProps = {
  listener: changes => changes
};

export default SignalRService;
