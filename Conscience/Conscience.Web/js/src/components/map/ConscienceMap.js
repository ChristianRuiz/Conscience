import React from 'react';
import { withRouter } from 'react-router-dom';
import { Map, Marker, Popup } from 'react-leaflet';
import { BingLayer } from 'react-leaflet-bing';
import $ from 'jquery';
import 'ms-signalr-client';

import HostPopup from './HostPopup';

class ConscienceMap extends React.Component {
  constructor(props) {
    super(props);

    this.addHost = this.addHost.bind(this);

    this.state = {
      defaultPosition: [37.048601, -2.4216117],
      hosts: []
    };

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

      const hosts = this.state.hosts.filter(h => h.userId !== userId);
      this.setState({ hosts });
    });

    connection.start()
    .done(() => {
      console.log(`Now connected, connection ID=${connection.id}`);
      proxy.invoke('subscribeWeb');
    })
    .fail(() => { console.log('Could not connect'); });
  }

  addHost(userId, userName, location) {
    const hosts = this.state.hosts.filter(h => h.userId !== userId);
    const host = { userId, userName };

    if (location) {
      host.location = [location.Latitude, location.Longitude];
    }

    this.setState({ hosts: hosts.concat(host) });
  }

  /* componentWillReceiveProps(newProps) {
    if (!newProps.data.loading) {
      newProps.data.hosts.getAll.forEach(h => {
        if (h.device && h.device.online)
        addHost(h.id, h.account.userName, h.device.currentLocation)
      });
    }
  }*/

  render() {
    return (<div>
      <Map center={this.state.defaultPosition} zoom={18} style={{ height: 500 }}>
        <BingLayer bingkey="Aqh7oaz-q_8iKzjPjvzPaac4jn2HAU7iPF36ftyQ9u6-34rJktZsKTO_JNJsHUKB" />
        {
          this.state.hosts.filter(h => h.location).map(host =>
            <Marker position={host.location} key={host.userId}>
              <Popup>
                <HostPopup host={host} />
              </Popup>
            </Marker>)
        }
      </Map>
    </div>);
  }
}

/* const query = gql`query GetHosts {
            hosts {
                getAll {
                        id,
                        account {
                            userName
                        },
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
      `;*/

export default withRouter(ConscienceMap);
