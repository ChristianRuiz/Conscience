import React from 'react';
import { Link, withRouter } from 'react-router-dom';
import { graphql, gql } from 'react-apollo';

import AccountPicture from '../common/AccountPicture';
import ScrollableContainer from '../common/ScrollableContainer';

import styles from '../../styles/components/info-panel/hostsInfoPanel.css';

class CharacterInfoPanel extends React.Component {
  render() {
    if (this.props.data.loading) {
      return (<div>Loading...</div>);
    }

    const character = this.props.data.characters.byId;

    if (character !== null) {
      return (<div className="infoPanel">
        <ScrollableContainer>
          <div>
            <Link to={`/character-detail/${character.id}`} >
              <AccountPicture pictureUrl={character.currentHost ? character.currentHost.host.account.pictureUrl : ''} />
            </Link>
            <div className="card">
              <Link to={`/character-detail/${character.id}`} ><h1 className="charName">{character.name}</h1></Link>
              <p className="serialNumber">{character.currentHost ? `#${character.currentHost.host.account.userName}` : '-'}</p>
              <p className="battery">{character.currentHost ? `${(character.currentHost.host.account.device.batteryLevel * 100).toString().substring(0, 3).replace('.', '')}%` : '-'}</p>
              <p className="narrativeFunction">{character.narrativeFunction}</p>
            </div>

            <div>
              <h3>Active Plotlines</h3>

              <ul>
                {character.plots.map(p =>
                  <li key={p.plot.id}><Link to={`/plot-detail/${p.plot.id}`} >{p.plot.name}</Link></li>)}
              </ul>
            </div>

            {character.currentHost ?
            (<div>
              <Link to={`/behaviour-detail/${character.currentHost.host.id}`} ><h3>Behaviour Stats</h3></Link>

              <ul>
                {character.currentHost.host.stats.slice(0, 6).map(stat => 
                // .sort((a, b) => Math.abs(a.value - 10) + Math.abs(b.value - 10))
                  <li key={stat.name}>{stat.name}: {stat.value}</li>)}
              </ul>
            </div>)
            : '' }

            {character.currentHost ?
            (<div>
              <Link to={`/security-host/${character.currentHost.host.id}`} ><h3>Log</h3></Link>
            </div>)
            : '' }
          </div>
        </ScrollableContainer>
      </div>);
    }

    return <div />;
  }
}

CharacterInfoPanel.propTypes = {
  data: React.PropTypes.object.isRequired
};

const query = gql`query GetCharacterInfoPanelDetails($characterId:Int!) {
  characters {
    byId(id:$characterId)
    {
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
      currentHost {
        id
        host {
          id
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
          stats {
            id,
            name,
            value
          }
        }
      }
    }
  }
}
      `;


export default withRouter(graphql(query)(CharacterInfoPanel));
