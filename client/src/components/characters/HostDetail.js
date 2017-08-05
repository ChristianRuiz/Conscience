import React from 'react';
import { Link, withRouter } from 'react-router-dom';
import { graphql, gql } from 'react-apollo';

import PictureDescriptionBox from '../common/PictureDescriptionBox';

class HostsDetail extends React.Component {
  render() {
    if (this.props.data.loading) {
      return (<div>Loading...</div>);
    }

    const host = this.props.data.hosts.byId;

    return (<div>
      <h1>{host.currentCharacter ? host.currentCharacter.character.name : ''}</h1>

      {host.currentCharacter ? (<div>
        <h2>Character History:</h2>
        <p>{host.currentCharacter.character.story}</p>
      </div>) : ''}

      {host.currentCharacter ? (<div>

        <h2>Triggers:</h2>

        <ul>
          {host.currentCharacter.character.triggers.map(trigger =>
            <li key={trigger.id}>{trigger.description}</li>)}
        </ul>

        <h2>Memories:</h2>

        <ul>
          {host.currentCharacter.character.memories.map(memory =>
            <li key={memory.id}>{memory.description}</li>)}
        </ul>

        <h2>Plots:</h2>

        <ul>
          {host.currentCharacter.character.plots.map(p =>
            <li key={p.plot.id}>
              <p><Link to={`/plot-detail/${p.plot.id}`} >{p.plot.name}</Link>: {p.plot.description}</p>
              <p><b>Character involvement: </b></p>
              <p>{p.description}</p>
            </li>)}
        </ul>

        {host.currentCharacter.character.plotEvents.length > 0 ? (<div>
          <h2>Plot events:</h2>

          <ul>
            {host.currentCharacter.character.plotEvents.map(event =>
              <li key={event.id}>
                <p><b>{event.description}</b></p>
                <p><b>Plot: </b>{event.plot.name}</p>
                <p><b>Location: </b>{event.location}</p>
                <p><b>Time: </b>{event.hour}:{event.minutes}</p>
              </li>)}
          </ul></div>) : '' }

        {host.currentCharacter.character.relations.length > 0 ? (<div>
          <h2>Relations:</h2>

          <div className="flexColumn marginTop">
            {host.currentCharacter.character.relations.map(relation =>
              <PictureDescriptionBox
                key={relation.id}
                pictureUrl={relation.character.currentHost.host.account.pictureUrl}
                title={relation.character.name}
                link={`/character-detail/${relation.character.currentHost.host.id}`}
                description={relation.description}
              />)}
          </div>
        </div>) : '' }

      </div>) : ''}
    </div>);
  }
}

HostsDetail.propTypes = {
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
          age,
          gender,
          story,
          narrativeFunction,
          memories {
            id,
            description
          },
          triggers {
            id,
            description
          },
          plots {
            id,
            plot {
              id,
              name,
              description
            },
            description
          },
          plotEvents {
            id,
            plot {
              id,
              name
            },
            description,
            location,
            hour,
            minute,
            characters {
              id,
              name
            }
          },
          relations {
            id,
            description,
            character {
              id,
              name
              currentHost {
                  host {
                    id
                    account {
                      id
                      pictureUrl
                    }
                  }
                }
            }
          }
        }
      },
      coreMemory1
      {
        id,
        audio { transcription },
        locked
      },
      coreMemory2
      {
        id,
        audio { transcription },
        locked
      },
      coreMemory3
      {
        id,
        audio { transcription },
        locked
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

export default withRouter(graphql(query)(HostsDetail));
