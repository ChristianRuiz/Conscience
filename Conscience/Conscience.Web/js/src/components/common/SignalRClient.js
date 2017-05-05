import React from 'react';
import { withApollo, gql } from 'react-apollo';
import $ from 'jquery';
import 'ms-signalr-client';

class SignalRClient extends React.Component {
  constructor(props) {
    super(props);

    this.addHost = this.addHost.bind(this);

    const connection = $.hubConnection('/signalr/hubs');
    const proxy = connection.createHubProxy('HostsHub');

    proxy.on('hostConnected', (userId, userName, location) => {
      console.log(`hostConnected ${userId} ${userName} ${JSON.stringify(location)}`);

      this.addHost(userId, userName, location);
    });

    proxy.on('locationUpdated', (userId, userName, location) => {
      console.log(`locationUpdated ${userId} ${userName} ${JSON.stringify(location)}`);

      this.addHost(userId, userName, location);
    });

    proxy.on('hostDisconnected', (userId) => {
      console.log(`hostDisconnected ${userId}`);

      // TODO: Implement
    });

    connection.start()
    .done(() => {
      console.log(`Now connected, connection ID=${connection.id}`);
      proxy.invoke('subscribeWeb');
    })
    .fail(() => { console.log('Could not connect'); });
  }

  addHost(userId, userName, location) {
    const query = gql`query GetHostsForMap {
            hosts {
                all {
                        id,
                        account {
                            userName,
                            device {
                              currentLocation {
                                latitude,
                                longitude
                              },
                              online
                            }
                        }
                }
            }
        }
    `;

    let data;

    try {
      data = this.props.client.readQuery({ query });
    } catch (e) {
      console.log('There are no hosts on the cache');
      return;
    }

    const host = data.hosts.all.find(h => h.id === userId);

    if (!host) {
      console.warn(`There is no host cached with id: ${userId}`);
      return;
    }

    if (location) {
      if (!host.account.device) {
        host.account.device = { currentLocation: {} };
      }

      host.account.device.currentLocation.latitude = location.Latitude;
      host.account.device.currentLocation.longitude = location.Longitude;
    }

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
