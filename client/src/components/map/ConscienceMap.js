import React from 'react';
import { withRouter } from 'react-router-dom';
import { graphql } from 'react-apollo';

import { Map, Marker, Popup } from 'react-leaflet';
import { BingLayer } from 'react-leaflet-bing';

import HostsInfoPanel from '../info-panel/HostsInfoPanel';
import HostPopup from './HostPopup';

import query from '../../queries/MapQuery';

class ConscienceMap extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      // defaultPosition: [37.048601, -2.4216117],
      defaultPosition: [37.1275825, -4.6669954],
      selectedAccount: null,
      height: parseInt(window.innerHeight)
    };
  }

  render() {
    if (this.props.data.loading) {
      return <div />;
    }

    return (<div className="mainContainer">
      <Map
        className="mainContent"
        center={this.state.defaultPosition}
        zoom={18}
        style={{ height: this.state.height - 110 }}
        onclick={() =>  this.setState({ selectedAccount: null })}
      >
        <BingLayer bingkey="Aqh7oaz-q_8iKzjPjvzPaac4jn2HAU7iPF36ftyQ9u6-34rJktZsKTO_JNJsHUKB" />
        {
          this.props.data.accounts.all
          .filter(account => account.host && account.device && account.device.currentLocation)
          .map(account =>
            <Marker
              key={account.id}
              position={[account.device.currentLocation.latitude,
                account.device.currentLocation.longitude]}
              onclick={() => this.setState({ selectedAccount: account })}
            >
              <Popup>
                <HostPopup account={account} />
              </Popup>
            </Marker>)
        }
      </Map>

      {this.state.selectedAccount ?
        <HostsInfoPanel hostId={this.state.selectedAccount.host.id} /> : <div /> }
    </div>);
  }
}

ConscienceMap.propTypes = {
  data: React.PropTypes.object.isRequired
};

export default withRouter(graphql(query)(ConscienceMap));
