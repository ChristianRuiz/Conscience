import React from 'react';
import { withRouter } from 'react-router-dom';
import $ from 'jquery';
import 'ms-signalr-client';
import RaisedButton from 'material-ui/RaisedButton';

class HostClientTest extends React.Component {
  constructor(props) {
    super(props);

    this._connect = this._connect.bind(this);
    this._updateLocation = this._updateLocation.bind(this);
    this._disconnect = this._disconnect.bind(this);

    this.state = {
      isConnected: false
    };

    this.connection = $.hubConnection('/signalr/hubs');
    this.proxy = this.connection.createHubProxy('HostsHub');
  }

  _connect() {
    this.connection.start()
    .done(() => {
      console.log(`Now connected, connection ID=${this.connection.id}`);
      this.proxy.invoke('subscribeHost', 'TestReactClient').done(() => { this.setState({ isConnected: true }); });
    })
    .fail(() => { console.log('Could not connect'); });
  }

  _updateLocation() {
    const location = { Latitude: 37.048601, Longitude: -2.4216117 };

    let random = Math.random() / 1000;

    location.Latitude += random;

    random = Math.random() / 1000;

    location.Longitude += random;

    this.proxy.invoke('locationUpdates', [location]);
  }

  _disconnect() {
    this.connection.stop();
    this.setState({ isConnected: false });
  }

  render() {
    if (!this.state.isConnected) {
      return <RaisedButton label="Connect" onClick={this._connect} />;
    }

    return (<div>
      <RaisedButton label="Disconnect" onClick={this._disconnect} />
      <RaisedButton label="Update Location" onClick={this._updateLocation} />
    </div>);
  }
}

export default withRouter(HostClientTest);
