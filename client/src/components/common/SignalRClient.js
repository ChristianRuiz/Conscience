import React from 'react';
import { withApollo, gql } from 'react-apollo';
import $ from 'jquery';
import 'ms-signalr-client';

import query from '../../queries/MapQuery';

class SignalRClient extends React.Component {
  constructor(props) {
    super(props);

    this.addAccount = this.addAccount.bind(this);
    this.reconnect = this.reconnect.bind(this);

    this.connection = $.hubConnection('/signalr/hubs');
    this.proxy = this.connection.createHubProxy('AccountsHub');

    this.proxy.on('accountConnected', (accountId, location) => {
      console.log(`accountConnected ${accountId} ${JSON.stringify(location)}`);

      this.addAccount(accountId, location);
    });

    this.proxy.on('locationUpdated', (accountId, location) => {
      console.log(`locationUpdated ${accountId} ${JSON.stringify(location)}`);

      this.addAccount(accountId, location);
    });

    this.proxy.on('accountDisconnected', (accountId) => {
      console.log(`accountDisconnected ${accountId}`);

      // TODO: Implement
    });

    this.proxy.on('broadcastError', (context, error) => {
      console.log(`Client error: ${context} \n ${error}`);
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

  addAccount(accountId, location) {
    let data;

    try {
      data = this.props.client.readQuery({ query });
    } catch (e) {
      console.log('There are no accounts on the cache');
      return;
    }

    const account = data.accounts.all.find(h => h.id === accountId);

    if (!account) {
      console.warn(`There is no account cached with id: ${accountId}`);
      return;
    }

    if (!location) {
      return;
    }

    if (!account.device || !account.device.currentLocation) {
      const updateQuery = gql`query UpdateAccount($accountId:Int!) {
          accounts {
            byId(id: $accountId) {
              id
              userName
              device {
                id
                currentLocation {
                  id
                  latitude
                  longitude
                }
                online
              }
            }
          }
        }
    `;

      this.props.client.query({
        query: updateQuery,
        variables: { accountId }
      });
      return;
    }

    account.device.currentLocation.latitude = location.Latitude;
    account.device.currentLocation.longitude = location.Longitude;

    this.props.client.writeQuery({ query, data });
  }

  render() {
    return <div />;
  }
}

SignalRClient.propTypes = {
  client: React.PropTypes.object.isRequired
};

export default withApollo(SignalRClient);
