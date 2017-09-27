import React from 'react';
import { withRouter } from 'react-router-dom';
import { graphql, gql } from 'react-apollo';

import styles from '../../styles/components/maintenance/maintenance.css';

class MaintenanceDetail extends React.Component {
  render() {
    if (this.props.data.loading) {
      return (<div>Loading...</div>);
    }

    const host = this.props.data.hosts.byId;

    return (<div className="mainContent flex">
      <div className="maintenanceDetailContainer">
        <div className="scanContainer">
          <div className="scan" />
        </div>
        <div className="statusContainer">
          <div className="statusBox">
            <h2>STATUS</h2>
            <p>NON CRITICAL - PROCEED REPAIRS</p>
          </div>
          <div className="statusCenter" />
          <div className="statusLines" />
        </div>
      </div>
    </div>);
  }
}

MaintenanceDetail.propTypes = {
  data: React.PropTypes.object.isRequired
};

const query = gql`query GetHostMaintenanceDetails($hostId:Int!) {
  hosts {
    byId(id:$hostId)
    {
      id,
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

export default withRouter(graphql(query)(MaintenanceDetail));
