import React from 'react';
import { View } from 'react-native';
import DeviceBattery from 'react-native-device-battery';
import DeviceInfo from 'react-native-device-info';
import signalr from 'react-native-signalr';
import { withApollo, gql } from 'react-apollo';

import Constants from '../../constants';

class SignalRService extends React.Component {
  constructor(props) {
    super(props);

    this._onTimer = this._onTimer.bind(this);

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
    });
  }

  componentDidMount() {
    this.connection = signalr.hubConnection(`${Constants.SERVER_URL}/signalr/hubs`);
    this.proxy = this.connection.createHubProxy('HostsHub');

    this.connection.start()
    .done(() => {
      console.log(`Now connected, connection ID=${this.connection.id}`);
      this.proxy.invoke('subscribeHost', this.deviceId).done(() => {
        setInterval(this._onTimer, 1000 * 15);
        this._onTimer();
      });
    })
    .fail(() => { console.log('Could not connect to SignalR'); });

    this.connection.disconnected(() => {
      setTimeout(() => {
        this.connection.start()
        .done(() => {
          console.log(`Now reconnected, connection ID=${this.connection.id}`);
          this.proxy.invoke('subscribeHost', this.deviceId);
        })
        .fail(() => { console.log('Could not connect'); });
      }, 5000); // Restart connection after 5 seconds.
    });
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
        Object.assign(update, {
          locations: this.locations
        });
      }

      if (this.locations) {
        const serviceLocations = this.locations.map(l => ({
          Latitude: l.coords.latitude,
          Longitude: l.coords.longitude,
          Accuracy: l.coords.accuracy,
          Altitude: l.coords.altitude,
          Heading: l.coords.heading,
          Speed: l.coords.speed,
          TimeStamp: new Date(l.timestamp).toISOString()
        }));

        try {
          // TODO: Consolidate locations
          this.proxy.invoke('locationUpdates', serviceLocations, this.charging ? 1 : 0, null, this.batteryLevel);
          this.locations = [];
        } catch (e) {
          console.warn(`Unable to send updates to SignalR: ${e}`);
        }

        if (serviceLocations.length > 0) {
          const currentLocation = serviceLocations[serviceLocations.length - 1];

          const query = gql`{
  accounts {
    current {
      device {
        deviceId,
        batteryLevel,
        batteryStatus,
        currentLocation {
          latitude,
          longitude,
          timeStamp
        }
      }
    }
  }
}`;

          let data;

          try {
            data = this.props.client.readQuery({ query });
          } catch (e) {
            console.warn('There is no current account on the cache');
            return;
          }

          if (!data.accounts.current.device) {
            data.accounts.current.device = {};
          }

          const device = data.accounts.current.device;

          device.deviceId = this.deviceId;
          device.batteryLevel = this.batteryLevel;
          device.batteryStatus = this.charging ? 'CHARGING' : 'NOT_CHARGING';
          device.currentLocation = {
            latitude: currentLocation.Latitude,
            longitude: currentLocation.Longitude,
            timeStamp: currentLocation.TimeStamp,
            __typename: 'Location'
          };

          this.props.client.writeQuery({ query, data });
        }
      }

      this.props.listener(update);
    }
  }

  render() {
    return <View />;
  }
}

SignalRService.propTypes = {
  client: React.PropTypes.object.isRequired,
  listener: React.PropTypes.func
};

SignalRService.defaultProps = {
  listener: changes => changes
};

export default withApollo(SignalRService);
