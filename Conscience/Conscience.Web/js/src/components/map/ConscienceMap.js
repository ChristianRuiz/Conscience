import React from 'react';
import { withRouter } from 'react-router-dom';
import { graphql, gql } from 'react-apollo';

import { Map, Marker, Popup } from 'react-leaflet';
import { BingLayer } from 'react-leaflet-bing';

import HostPopup from './HostPopup';

class ConscienceMap extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      defaultPosition: [37.048601, -2.4216117]
    };
  }

  render() {
    if (this.props.data.loading) {
      return <div />;
    }

    return (<div>
      <Map center={this.state.defaultPosition} zoom={18} style={{ height: 500 }}>
        <BingLayer bingkey="Aqh7oaz-q_8iKzjPjvzPaac4jn2HAU7iPF36ftyQ9u6-34rJktZsKTO_JNJsHUKB" />
        {
          this.props.data.hosts.all
          .filter(host => host.account.device && host.account.device.currentLocation)
          .map(host =>
            <Marker
              key={host.id}
              position={[host.account.device.currentLocation.latitude,
                host.account.device.currentLocation.longitude]}
            >
              <Popup>
                <HostPopup host={host} />
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

export default withRouter(graphql(query)(ConscienceMap));
