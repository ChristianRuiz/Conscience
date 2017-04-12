import React from 'react';
import { withRouter } from 'react-router-dom';
import { Map, Marker, Popup } from 'react-leaflet';
import { BingLayer } from 'react-leaflet-bing';
import $ from 'jquery';
import 'ms-signalr-client';

class ConscienceMap extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      hosts: [{
        location: {
          latitude: 37.048601,
          longitude: -2.4216117
        }
      }]
    };

    const connection = $.hubConnection('/signalr/hubs');
    const proxy = connection.createHubProxy('HostsHub');

    proxy.on('hostConnected', (user) => {
      console.log(`hostConnected ${JSON.stringify(user)}`);
    });

    proxy.on('locationUpdated', (userId, userName, location) => {
      console.log(`locationUpdated ${userId} ${userName} ${location}`);
    });

    proxy.on('hostDisconnected', (userId) => {
      console.log(`hostDisconnected ${userId}`);
    });

    connection.start()
    .done(() => {
      console.log(`Now connected, connection ID=${connection.id}`);
      proxy.invoke('subscribeWeb');
    })
    .fail(() => { console.log('Could not connect'); });
  }

  render() {
    const position = [
      this.state.hosts[0].location.latitude,
      this.state.hosts[0].location.longitude
    ];

    return (<div>
      <Map center={position} zoom={18} style={{ height: 500 }}>
        <BingLayer bingkey="Aqh7oaz-q_8iKzjPjvzPaac4jn2HAU7iPF36ftyQ9u6-34rJktZsKTO_JNJsHUKB" />
        <Marker position={position}>
          <Popup>
            <span>A pretty CSS3 popup. <br /> Easily customizable.</span>
          </Popup>
        </Marker>
      </Map>
    </div>);
  }
}

export default withRouter(ConscienceMap);
