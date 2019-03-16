import React from 'react';
import { withRouter } from 'react-router-dom';
import { graphql, withApollo } from 'react-apollo';

import { icon } from 'leaflet';
import { Map, Marker } from 'react-leaflet';
import { BingLayer } from 'react-leaflet-bing';
import TextField from 'material-ui/TextField';

import HostsOrCharacterInfoPanel from '../info-panel/HostsOrCharacterInfoPanel';
import EmployeesInfoPanel from '../info-panel/EmployeesInfoPanel';

import query from '../../queries/MapQuery';

import style from '../../styles/components/map/map.css';
import { setInterval } from 'timers';

class ConscienceMap extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      defaultPosition: [37.048601, -2.4216117],
      selectedAccount: null,
      height: parseInt(window.innerHeight),
      search: ''
    };

    setInterval(() => {
      props.client.query({
        fetchPolicy: 'network-only',
        fetchResults: true,
        query
      });
    }, 1000 * 15);
  }

  getMarkerIcon(state, account) {
    const markerBase = '/content/images/map';

    let markerIcon = `${markerBase}/markerAleph.png`;

    if (account.host && !account.employee) {
      if (account.host.status === 'OK') {
        markerIcon = `${markerBase}/markerHost.png`;
      } else if (account.host.status === 'HURT') {
        markerIcon = `${markerBase}/markerHurt.png`;
      } else if (account.host.status === 'DEAD') {
        markerIcon = `${markerBase}/markerDead.png`;
      }
    }

    if (state.selectedAccount && state.selectedAccount.id === account.id) {
      markerIcon = `${markerBase}/markerSelected.png`;
    }

    return markerIcon;
  }

  getMarkerZIndex(state, account) {
    let zindex = 100;

    if (account.host) {
      if (account.host.status === 'OK') {
        zindex = 200;
      } else if (account.host.status === 'HURT') {
        zindex = 300;
      } else if (account.host.status === 'DEAD') {
        zindex = 400;
      }
    }

    if (state.selectedAccount && state.selectedAccount.id === account.id) {
      zindex = 500;
    }

    return zindex;
  }

  render() {
    if (this.props.data.loading) {
      return <div />;
    }

    let hostPanel = <div />;

    if (this.state.selectedAccount && this.state.selectedAccount.host) {
      hostPanel = <HostsOrCharacterInfoPanel hostId={this.state.selectedAccount.host.id} />;
    }

    return (<div className="mainContainer">
      <Map
        className="mainContent"
        center={this.state.defaultPosition}
        zoom={18}
        style={{ height: this.state.height - 110 }}
        onclick={() => this.setState({ selectedAccount: null })}
      >
        <BingLayer bingkey="Aqh7oaz-q_8iKzjPjvzPaac4jn2HAU7iPF36ftyQ9u6-34rJktZsKTO_JNJsHUKB" />

        {
          this.props.data.accounts.all
          .filter(account =>
          account.device && account.device.currentLocation

          && ((account.host && account.host.currentCharacter
          && ((this.state.search.trim() === ''
            || account.userName.toLowerCase().indexOf(this.state.search.toLowerCase()) >= 0
            || account.host.currentCharacter.character.name.toLowerCase()
                                .indexOf(this.state.search.toLowerCase()) >= 0)))

          || (account.employee
            && (this.state.search.trim() === ''
              || account.userName.toLowerCase().indexOf(this.state.search.toLowerCase()) >= 0
              || account.employee.name.toLowerCase()
                          .indexOf(this.state.search.toLowerCase()) >= 0))))
          .map(account =>
            <Marker
              key={account.id}
              position={[account.device.currentLocation.latitude,
                account.device.currentLocation.longitude]}
              onclick={() => this.setState({ selectedAccount: account })}
              icon={icon({ iconUrl: this.getMarkerIcon(this.state, account) })}
              zIndexOffset={this.getMarkerZIndex(this.state, account)}
            />)
        }
      </Map>

      <div className="searchText">
        <TextField
          hintText="Search..."
          value={this.state.search}
          onChange={e => this.setState({ search: e.target.value })}
        />
      </div>

      { hostPanel }

      {this.state.selectedAccount && this.state.selectedAccount.employee ?
        <EmployeesInfoPanel employeeId={this.state.selectedAccount.employee.id} /> : <div /> }
    </div>);
  }
}

ConscienceMap.propTypes = {
  data: React.PropTypes.object.isRequired,
  client: React.PropTypes.object.isRequired
};

export default withRouter(withApollo(graphql(query, { options: { fetchPolicy: 'network-only' } })(ConscienceMap)));
