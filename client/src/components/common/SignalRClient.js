import React from 'react';
import { withApollo, gql } from 'react-apollo';
import $ from 'jquery';
import 'ms-signalr-client';

import queryMap from '../../queries/MapQuery';
import queryCharacterDetail from '../../queries/CharacterDetailQuery';
import queryHostStats from '../../queries/HostStatsQuery';

class SignalRClient extends React.Component {
  constructor(props) {
    super(props);

    this.updateAccount = this.updateAccount.bind(this);
    this.reconnect = this.reconnect.bind(this);

    this.connection = $.hubConnection('/signalr/hubs');
    this.proxy = this.connection.createHubProxy('AccountsHub');

    this.proxy.on('broadcastError', (context, error) => {
      console.log(`Client error: ${context} \n ${error}`);
    });

    this.proxy.on('locationUpdated', (accountId, location, batteryLevel, batteryStatus, lastConnection, hostStatus) => {
      this.updateAccount(accountId, location, batteryLevel, batteryStatus, lastConnection, hostStatus);
    });

    this.proxy.on('characterUpdated', (characterId) => {
      this.props.client.query({
        fetchPolicy: 'network-only',
        fetchResults: true,
        query: queryCharacterDetail,
        variables: { characterId }
      });
    });

    this.proxy.on('statsModified', (hostId) => {
      this.props.client.query({
        fetchPolicy: 'network-only',
        fetchResults: true,
        query: queryHostStats,
        variables: { hostId }
      });
    });

    this.proxy.on('panicButton', (notificationId) => {
      // TODO: Alarm sound
      alert('Panic button! Check notifications!');
      console.warn(`Panic button pressed with notification: ${notificationId}`);
    });

    this.connection.start()
    .done(() => {
      console.log(`Now connected, connection ID=${this.connection.id}`);
      this.proxy.invoke('subscribeWeb');
    })
    .fail(() => { console.log('Could not connect'); });

    this.connection.disconnected(() => {
      this.reconnect();
    });
  }

  reconnect() {
    setTimeout(() => {
      this.connection.start()
        .done(() => {
          console.log(`Now reconnected, connection ID=${this.connection.id}`);
          this.proxy.invoke('subscribeWeb');
        })
        .fail(() => { this.reconnect(); });
    }, 5000); // Restart connection after 5 seconds.
  }

  updateAccount(accountId, location, batteryLevel, batteryStatus, lastConnection, hostStatus) {
    let data;

    try {
      data = this.props.client.readQuery({ queryMap });
    } catch (e) {
      console.log(`The account ${accountId} is not cached, quering the server for it`);
      this.props.client.query({
        fetchPolicy: 'network-only',
        fetchResults: true,
        query: queryMap
      });
      return;
    }

    const account = data.accounts.all.find(h => h.id === accountId);

    if (!account || !account.device || !account.device.currentLocation) {
      console.warn(`There is no account cached with id: ${accountId}, quering the server for it`);
      this.props.client.query({
        fetchPolicy: 'network-only',
        fetchResults: true,
        query: queryMap
      });
      return;
    }

    account.device.batteryLevel = batteryLevel;
    account.device.batteryStatus = batteryStatus;
    account.device.lastConnection = lastConnection;

    if (location) {
      account.device.currentLocation.latitude = location.Latitude;
      account.device.currentLocation.longitude = location.Longitude;
    }

    if (account.host) {
      account.host.status = hostStatus;
    }

    this.props.client.writeQuery({ query: queryMap, data });
  }

  render() {
    return <div />;
  }
}

SignalRClient.propTypes = {
  client: React.PropTypes.object.isRequired
};

export default withApollo(SignalRClient);
