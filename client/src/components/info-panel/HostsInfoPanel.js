import React from 'react';
import { Link, withRouter } from 'react-router-dom';
import { graphql, gql } from 'react-apollo';

import AccountPicture from '../common/AccountPicture';

import styles from '../../styles/components/info-panel/hostsInfoPanel.css';

class HostsInfoPanel extends React.Component {
  render() {
    if (this.props.data.loading) {
      return (<div>Loading...</div>);
    }

    const host = this.props.data.hosts.byId;

    if (host !== null) {
      return (<div className="infoPanel">
        <Link to={`/character-detail/${host.id}`} ><AccountPicture pictureUrl={host.account.pictureUrl} /></Link>
        <div className="card">
          <Link to={`/character-detail/${host.id}`} ><h1 className="charName">{host.currentCharacter ? host.currentCharacter.character.name : ''}</h1></Link>
          <p className="serialNumber">#{host.account.userName}</p>
          <p className="online">{host.account.device.online ? 'On Line' : 'Off Line'}</p>
        </div>

        <div>
          <Link to={`/behaviour-detail/${host.id}`} ><h3>Behaviour Stats</h3></Link>

          <ul>
            {host.stats.filter(stat => stat.value !== 10).map(stat =>
              <li key={stat.name}>{stat.name}: {stat.value}</li>)}
          </ul>
        </div>
      </div>);
    }

    return <div />;
  }
}

HostsInfoPanel.propTypes = {
  data: React.PropTypes.object.isRequired
};

const query = gql`query GetHostDetails($hostId:Int!) {
  hosts {
    byId(id:$hostId)
    {
      id,
      account {
        id,
        userName,
        pictureUrl,
        device {
          id,
          lastConnection,
          online,
          batteryLevel
        }
      },
      currentCharacter {
        id,
        assignedOn,
        character {
          id,
          name,
          narrativeFunction,
          plots {
            id,
            plot {
              id,
              name,
              description
            },
            description
          }
        }
      },
      stats {
        id,
        name,
        value
      }
    }
  }
}
      `;


export default withRouter(graphql(query)(HostsInfoPanel));
