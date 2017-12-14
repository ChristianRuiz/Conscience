import React from 'react';
import { Link, withRouter } from 'react-router-dom';
import { graphql, gql } from 'react-apollo';

import AccountPicture from '../common/AccountPicture';
import ScrollableContainer from '../common/ScrollableContainer';

import styles from '../../styles/components/info-panel/hostsInfoPanel.css';

class HostsInfoPanel extends React.Component {
  render() {
    if (this.props.data.loading) {
      return (<div>Loading...</div>);
    }

    const host = this.props.data.hosts.byId;

    if (host !== null) {
      return (<div className="infoPanel">
        <ScrollableContainer>
          <div>
            <AccountPicture pictureUrl={host.account.pictureUrl} />
            <div className="card">
              <p className="serialNumber">{host.account.userName}</p>
              <p className="battery">{`${(host.account.device.batteryLevel * 100).toString().substring(0, 3).replace('.', '')}%`}</p>
            </div>

            <div>
              <Link to={`/behaviour-detail/${host.id}`} ><h3>Behaviour Stats</h3></Link>

              <ul>
                {host.stats.slice(0, 6).map(stat =>
                  <li key={stat.name}>{stat.name}: {stat.value}</li>)}
              </ul>
            </div>
          </div>
        </ScrollableContainer>
      </div>);
    }

    return <div />;
  }
}

HostsInfoPanel.propTypes = {
  data: React.PropTypes.object.isRequired
};

const query = gql`query GetHostPanelDetails($hostId:Int!) {
  hosts {
    byId(id: $hostId)
    {
      id
      account {
        id
        userName
        pictureUrl
        device {
          id
          batteryLevel
        }
      }
      stats {
        id
        name
        value
      }
    }
  }
}
      `;


export default withRouter(graphql(query)(HostsInfoPanel));
