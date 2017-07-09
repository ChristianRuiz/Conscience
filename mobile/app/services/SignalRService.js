import { Platform } from 'react-native';
import BackgroundTimer from 'react-native-background-timer';
import DeviceBattery from 'react-native-device-battery';
import DeviceInfo from 'react-native-device-info';
import signalr from 'react-native-signalr';
import { gql } from 'react-apollo';
import Location from 'react-native-location';
import reportException from '../services/ReportException';

import Constants from '../constants';

class SignalRService {
  constructor(apolloClient, navigator, audioService) {
    this.client = apolloClient;
    this.audioService = audioService;

    this._onTimer = this._onTimer.bind(this);
    this._connect = this._connect.bind(this);
    this.reconnect = this.reconnect.bind(this);

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

    this._connect();
  }

  reconnect() {
    if (!this.reconnecting) {
      this.reconnecting = true;

      // this.audioService.playSound('2.mp3');

      reportException('Reconnecting SignalR', false);

      BackgroundTimer.setTimeout(() => {
        this._connect();
      }, 5000); // Restart connection after 5 seconds.
    }
  }

  _connect() {
    // this.audioService.playSound('3.mp3');

    if (this.connection) {
      try {
        this.connection.stop();
      } catch (e) {}
    }

    this.connection = signalr.hubConnection(`${Constants.SERVER_URL}/signalr/hubs`);
    this.proxy = this.connection.createHubProxy('AccountsHub');
    this.connection.start()
      .done(() => {
        console.log(`Now reconnected, connection ID=${this.connection.id}`);
        this.proxy.invoke('subscribeHost', this.deviceId).done(() => {
          this.reconnecting = false;

          if (this.intervalId) {
            BackgroundTimer.clearInterval(this.intervalId);
          }

          this.intervalId = BackgroundTimer.setInterval(() => {
            this._onTimer();
          }, 1000 * 15);

          this._onTimer();
        });
      })
      .fail((error) => {
        this.reconnecting = false;
        reportException(error, false);
        this.reconnect();
      });

    this.connection.disconnected(() => {
      this.reconnect();
    });
  }

  _onTimer() {
    // Hack: playing a sound on every timer tick to avoid Android OS to shut us down
    this.audioService.playSound('empty.mp3');
    reportException('Timer tick', false);

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
        this.proxy.invoke('locationUpdates', serviceLocations, this.charging ? 1 : 0, this.batteryLevel).then(() => {
          this.locations = [];
        }).fail((error) => {
          console.log(`Unable to to send location updates to SignalR ${error}`);
          this.reconnect();
        });
      } catch (e) {
        console.log(`Unable to send updates to SignalR: ${e}`);
        this.reconnect();
      }

      if (serviceLocations.length > 0) {
        const currentLocation = serviceLocations[serviceLocations.length - 1];

        const query = gql`{
accounts {
  current {
    id,
    device {
      id,
      deviceId,
      batteryLevel,
      batteryStatus,
      currentLocation {
        id,
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
          data = this.client.readQuery({ query });
        } catch (e) {
          console.log('There is no current account on the cache');
          return;
        }

        if (!data.accounts.current.device || !data.accounts.current.device.currentLocation) {
          const updateQuery = gql`query UpdateCurrentAccount {
                accounts {
                  current {
                    id,
                    device {
                      id,
                      deviceId,
                      batteryLevel,
                      batteryStatus,
                      currentLocation {
                        id,
                        latitude,
                        longitude,
                        timeStamp
                      }
                    }
                  }
                }
              }
        `;

          this.client.query({
            query: updateQuery
          });

          return;
        }

        const device = data.accounts.current.device;

        device.deviceId = this.deviceId;
        device.batteryLevel = this.batteryLevel;
        device.batteryStatus = this.charging ? 'CHARGING' : 'NOT_CHARGING';

        device.currentLocation.latitude = currentLocation.Latitude;
        device.currentLocation.longitude = currentLocation.Longitude;
        device.currentLocation.timeStamp = currentLocation.TimeStamp;

        this.client.writeQuery({ query, data });
      }
    }
  }
}

export default SignalRService;
