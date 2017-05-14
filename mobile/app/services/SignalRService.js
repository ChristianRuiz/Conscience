import React from 'react';
import DeviceBattery from 'react-native-device-battery';
import DeviceInfo from 'react-native-device-info';
import signalr from 'react-native-signalr';
import { gql } from 'react-apollo';

import Constants from '../constants';

class SignalRService {
  constructor(apolloClient, navigator, audioService) {
    this.client = apolloClient;

    this._onTimer = this._onTimer.bind(this);

    this.audioService = audioService;

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

    this.connection = signalr.hubConnection(`${Constants.SERVER_URL}/signalr/hubs`);
    this.proxy = this.connection.createHubProxy('AccountsHub');

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
          console.warn(`Unable to to send location updates to SignalR ${error}`);
        });
      } catch (e) {
        console.warn(`Unable to send updates to SignalR: ${e}`);
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
          console.warn('There is no current account on the cache');
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
