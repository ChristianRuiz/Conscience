import React from 'react';
import { Link, withRouter } from 'react-router-dom';
import { graphql, gql } from 'react-apollo';

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
          {host.currentCharacter.character.plots.map(plot =>
            <li key={plot.id}>
              <p><b>Plot: </b> {plot.plot.name}</p>
              <p><b>Plot description: </b></p>
              <p>{plot.plot.description}</p>
              <p><b>Character involvement: </b></p>
              <p>{plot.description}</p>
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

          <ul>
            {host.currentCharacter.character.relations.map(relation =>
              <li key={relation.id}>
                <p>
                  <Link to={`/character-detail/${relation.character.currentHost.host.id}`} ><b>{relation.character.name}: </b></Link>
                  {relation.description}
                </p>
              </li>)}
          </ul></div>) : '' }

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
