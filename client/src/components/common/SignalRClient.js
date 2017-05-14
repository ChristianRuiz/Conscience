import React from 'react';
import { withApollo, gql } from 'react-apollo';
import $ from 'jquery';
import 'ms-signalr-client';

class SignalRClient extends React.Component {
  constructor(props) {
    super(props);

    this.addAccount = this.addAccount.bind(this);

    const connection = $.hubConnection('/signalr/hubs');
    const proxy = connection.createHubProxy('AccountsHub');

    proxy.on('accountConnected', (accountId, location) => {
      console.log(`accountConnected ${accountId} ${JSON.stringify(location)}`);

      this.addAccount(accountId, location);
    });

    proxy.on('locationUpdated', (accountId, location) => {
      console.log(`locationUpdated ${accountId} ${JSON.stringify(location)}`);

      this.addAccount(accountId, location);
    });

    proxy.on('accountDisconnected', (accountId) => {
      console.log(`accountDisconnected ${accountId}`);

      // TODO: Implement
    });

    connection.start()
    .done(() => {
      console.log(`Now connected, connection ID=${connection.id}`);
      proxy.invoke('subscribeWeb');
    })
    .fail(() => { console.log('Could not connect'); });

    connection.disconnected(() => {
      setTimeout(() => {
        connection.start()
        .done(() => {
          console.log(`Now reconnected, connection ID=${connection.id}`);
          proxy.invoke('subscribeWeb');
        })
        .fail(() => { console.log('Could not connect'); });
      }, 5000); // Restart connection after 5 seconds.
    });
  }

  addAccount(accountId, location) {
    const query = gql`query GetAccountsForMap {
        accounts {
          all {
            id
            userName
            device {
              id
              currentLocation {
                latitude
                longitude
              }
              online
            }
          }
        }
      }
    `;

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
