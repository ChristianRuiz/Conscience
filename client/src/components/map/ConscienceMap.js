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
      defaultPosition: [37.048601, -2.4216117],
      selectedHost: null
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

const query = gql`query GetAllAccountsForMap
        {
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
              },
              host {
                id,
                currentCharacter {
                  id,
                  character {
                    id,
                    name
                  }
                }
              },
              employee {
                id,
                name
              }
            }
          }
        }
      `;

export default withRouter(graphql(query)(ConscienceMap));
