import React from 'react';
import { withRouter } from 'react-router-dom';
import { graphql } from 'react-apollo';

import { icon } from 'leaflet';
import { Map, Marker, Popup } from 'react-leaflet';
import { BingLayer } from 'react-leaflet-bing';

import HostsInfoPanel from '../info-panel/HostsInfoPanel';
import EmployeesInfoPanel from '../info-panel/EmployeesInfoPanel';

import TextField from 'material-ui/TextField';

import query from '../../queries/MapQuery';

import style from '../../styles/components/map/map.css';

class ConscienceMap extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      defaultPosition: [37.048601, -2.4216117],
      // defaultPosition: [37.1275825, -4.6669954],
      selectedAccount: null,
      height: parseInt(window.innerHeight),
      search: ''
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
              icon={icon({ iconUrl: this.state.selectedAccount === account ? '/content/images/map/markerSelected.png' :
              (account.host ? '/content/images/map/markerHost.png' : '/content/images/map/markerAleph.png') })}
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

      {this.state.selectedAccount && this.state.selectedAccount.host ?
        <HostsInfoPanel hostId={this.state.selectedAccount.host.id} /> : <div /> }

      {this.state.selectedAccount && this.state.selectedAccount.employee ?
        <EmployeesInfoPanel employeeId={this.state.selectedAccount.employee.id} /> : <div /> }
    </div>);
  }
}

ConscienceMap.propTypes = {
  data: React.PropTypes.object.isRequired
};

export default withRouter(graphql(query)(ConscienceMap));
