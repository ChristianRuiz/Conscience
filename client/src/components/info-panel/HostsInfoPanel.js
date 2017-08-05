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
          <p className="battery">{(host.account.device.batteryLevel * 100).toString().substring(0, 3).replace('.', '')} %</p>
          <p className="narrativeFunction">{host.currentCharacter ? host.currentCharacter.character.narrativeFunction : ''}</p>
        </div>

        {host.currentCharacter ?
        (<div>
          <h3>Active Plotlines</h3>

          <ul>
            {host.currentCharacter.character.plots.map(p =>
              <li key={p.plot.id}><Link to={`/plot-detail/${p.plot.id}`} >{p.plot.name}</Link></li>)}
          </ul>
        </div>)
        : '' }

        <div>
          <Link to={`/behaviour-detail/${host.id}`} ><h3>Behaviour Stats</h3></Link>

          <ul>
            {host.stats.slice(0, 6).map(stat => // .sort((a, b) => Math.abs(a.value - 10) + Math.abs(b.value - 10))
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
