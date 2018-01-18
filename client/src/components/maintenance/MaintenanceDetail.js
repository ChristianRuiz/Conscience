import React from 'react';
import { withRouter } from 'react-router-dom';
import { graphql, gql, compose } from 'react-apollo';

import Checkbox from 'material-ui/Checkbox';

import ScrollableContainer from '../common/ScrollableContainer';
import CriticalFailureCheck from './CriticalFailureCheck';

import styles from '../../styles/components/maintenance/maintenance.css';
import { setTimeout } from 'timers';

const checkStyle = { fill: 'white' };

class MaintenanceDetail extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      checksHeight: 0
    };
  }

  componentWillUpdate(props, state) {
    if (state.check1 && state.check2 && state.check3 && state.check4) {
      props.mutate({ hostId: props.data.hosts.byId.id });
      this.setState({ fixed: true, check1: false, check2: false, check3: false, check4: false });
    }
  }

  setChecksHeight(checksDiv) {
    if (!this.state.checksHeight && checksDiv) {
      setTimeout(() => this.setState({ checksHeight: checksDiv.clientHeight }), 1000);
    }
  }

  render() {
    if (this.props.data.loading) {
      return (<div>Loading...</div>);
    }

    const host = this.props.data.hosts.byId;
    
    let statusText = '';

    if (host.status === 'OK') {
      statusText = 'NO REPAIRS NEEDED';
    } else if (host.status === 'HURT') {
      statusText = 'NON CRITICAL - PROCEED REPAIRS';
    } else if (host.status === 'DEAD') {
      statusText = 'CRITICAL - PROCEED REPAIRS';
    }

    return (<div className="mainContent flex">
      <div className="maintenanceDetailContainer">
        <div className="scanContainer">
          <div className="scanContainerVertical">
            <div className="scanSpace" />
            <div className="scan" />
            <div className="scanSpace" />
          </div>
        </div>
        <div className="statusContainer">
          <div className="statusBox">
            <h2>STATUS</h2>
            <p>{ statusText }</p>
          </div>
          <div className="statusCenter" ref={checksDiv => this.setChecksHeight(checksDiv)}>
            <ScrollableContainer parentHeight={this.state.checksHeight} extraMargin={-100}>
            {host.status === 'OK' && this.state.fixed ?
              (<div><span>Host fixed, keep up the good work!</span></div>) :
              (host.status !== 'OK' ? (<div>
                <Checkbox
                  iconStyle={checkStyle}
                  label="Check the motor functions"
                  onCheck={() => this.setState({ check1: true })}
                  className="checkbox"
                />
                <Checkbox
                  iconStyle={checkStyle}
                  label="Check the visual fuction system"
                  onCheck={() => this.setState({ check2: true })}
                  className="checkbox"
                />
                <Checkbox
                  iconStyle={checkStyle}
                  label="Check the language intel processor"
                  onCheck={() => this.setState({ check3: true })}
                  className="checkbox"
                />
                <Checkbox
                  iconStyle={checkStyle}
                  label="Check the feelings command"
                  onCheck={() => this.setState({ check4: true })}
                  className="checkbox"
                />
              </div>) : <div />) }
            </ScrollableContainer>
          </div>
          <div className="statusLines">
            <div className="statusVitals1" />
            <div className="statusVitals2" />
          </div>
        </div>
      </div>
      {host.status !== 'OK' && this.state.check1 ?
        <CriticalFailureCheck hostId={host.id} /> : '' }
    </div>);
  }
}

MaintenanceDetail.propTypes = {
  data: React.PropTypes.object.isRequired,
  mutate: React.PropTypes.func.isRequired
};

const query = gql`query GetHostMaintenanceDetails($hostId:Int!) {
  hosts {
    byId(id:$hostId)
    {
      id,
      status,
      account {
        id,
        userName
      },
      currentCharacter {
        id,
        character {
          id,
          name
        }
      }
    }
  }
}
      `;

const mutation = gql`
mutation HostFixed($hostId:Int!) {
  hosts {
    fixed(hostId:$hostId){
      id
      status
    }
  }
}
`;

export default withRouter(compose(graphql(query), graphql(mutation))(MaintenanceDetail));
