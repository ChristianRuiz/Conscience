import React from 'react';
import { withRouter } from 'react-router-dom';
import { graphql } from 'react-apollo';

import { Map, Marker, Popup } from 'react-leaflet';
import { BingLayer } from 'react-leaflet-bing';

import InfoPanel from './InfoPanel';
import HostPopup from './HostPopup';

import query from '../../queries/MapQuery';

import styles from '../../styles/components/map/map.css';

class ConscienceMap extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      defaultPosition: [37.048601, -2.4216117],
      selectedAccount: null,
      height: parseInt(window.innerHeight)
    };
  }

  render() {
    if (this.props.data.loading) {
      return <div />;
    }

    return (<div className="mapContainer">
      <Map
        className="map"
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

      <InfoPanel account={this.state.selectedAccount} />
    </div>);
  }
}

ConscienceMap.propTypes = {
  data: React.PropTypes.object.isRequired
};

export default withRouter(graphql(query)(ConscienceMap));
