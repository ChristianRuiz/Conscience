import React from 'react';
import { withRouter } from 'react-router-dom';
import { graphql } from 'react-apollo';

import { Map, Marker, Popup } from 'react-leaflet';
import { BingLayer } from 'react-leaflet-bing';

import HostPopup from './HostPopup';

import query from '../../queries/MapQuery';

import styles from '../../styles/components/map/map.css';

class ConscienceMap extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      defaultPosition: [37.048601, -2.4216117],
      selectedHost: null,
      height: 0
    };
  }

  componentDidMount() {
    this.updateWindowDimensions();
    window.addEventListener('resize', this.updateWindowDimensions);
  }

  componentWillUnmount() {
    window.removeEventListener('resize', this.updateWindowDimensions);
  }

  updateWindowDimensions() {
    this.setState({ height: parseInt(window.innerHeight) });
  }

  render() {
    if (this.props.data.loading) {
      return <div />;
    }

    return (<div>
      <Map className="map" center={this.state.defaultPosition} zoom={18} style={{ height: this.state.height - 110 }}>
        <BingLayer bingkey="Aqh7oaz-q_8iKzjPjvzPaac4jn2HAU7iPF36ftyQ9u6-34rJktZsKTO_JNJsHUKB" />
        {
          this.props.data.accounts.all
          .filter(account => account.host && account.device && account.device.currentLocation)
          .map(account =>
            <Marker
              key={account.id}
              position={[account.device.currentLocation.latitude,
                account.device.currentLocation.longitude]}
            >
              <Popup>
                <HostPopup account={account} />
              </Popup>
            </Marker>)
        }
      </Map>
    </div>);
  }
}

ConscienceMap.propTypes = {
  data: React.PropTypes.object.isRequired
};

export default withRouter(graphql(query)(ConscienceMap));
