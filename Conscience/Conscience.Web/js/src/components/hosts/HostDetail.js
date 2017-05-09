import React from 'react';
import { withRouter } from 'react-router-dom';
import { graphql, gql } from 'react-apollo';

class HostsDetail extends React.Component {
  render() {
    if (this.props.data.loading) {
      return (<div>Loading...</div>);
    }

    const host = this.props.data.hosts.byId;

    return (<div>
      <div>{host.account.userName}</div>

      {host.currentCharacter ? <div>{host.currentCharacter.character.name}</div> : ''}

      {host.currentCharacter ? (<div>
        <p>Age: {host.currentCharacter.character.age}</p>
        <p>Gender: {host.currentCharacter.character.gender}</p>
        <h3>Story</h3>
        <p>{host.currentCharacter.character.story}</p>
        <div>
          <b>Narrative function: </b>{host.currentCharacter.character.narrativeFunction}
        </div>
      </div>) : ''}

      <h3>Stats summary</h3>

      <ul>
        {host.stats.filter(stat => stat.value !== 10).map(stat =>
          <li id={stat.name}><b>{stat.name}: </b> {stat.value}</li>)}
      </ul>

      {host.currentCharacter ? (<div>

        <h3>Memories</h3>

        <ul>
          {host.currentCharacter.character.memories.map(memory =>
            <li id={memory.id}>{memory.description}</li>)}
        </ul>

        <h3>Triggers</h3>

        <ul>
          {host.currentCharacter.character.triggers.map(trigger =>
            <li id={trigger.id}>{trigger.description}</li>)}
        </ul>

        <h3>Plots</h3>

        <ul>
          {host.currentCharacter.character.plots.map(plot =>
            <li id={plot.id}>
              <p><b>Plot: </b> {plot.plot.name}</p>
              <p><b>Plot description: </b></p>
              <p>{plot.plot.description}</p>
              <p><b>Character involvement: </b></p>
              <p>{plot.description}</p>
            </li>)}
        </ul>

        {host.currentCharacter.character.plotEvents.length > 0 ? (<div>
          <h3>Plot events</h3>

          <ul>
            {host.currentCharacter.character.plotEvents.map(event =>
              <li id={event.id}>
                <p><b>{event.description}</b></p>
                <p><b>Plot: </b>{event.plot.name}</p>
                <p><b>Location: </b>{event.location}</p>
                <p><b>Time: </b>{event.hour}:{event.minutes}</p>
              </li>)}
          </ul></div>) : '' }

        {host.currentCharacter.character.relations.length > 0 ? (<div>
          <h3>Relations</h3>

          <ul>
            {host.currentCharacter.character.relations.map(relation =>
              <li id={relation.id}>
                <p><b>{relation.character.name}: </b>{relation.description}</p>
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
        userName,
        device {
          lastConnection,
          online,
          batteryLevel
        }
      },
      currentCharacter {
        assignedOn,
        character {
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
              name,
              description
            },
            description
          },
          plotEvents {
            id,
            plot {
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
              name
            }
          }
        }
      },
      coreMemory1
      {
        audio { transcription },
        locked
      },
      coreMemory2
      {
        audio { transcription },
        locked
      },
      coreMemory3
      {
        audio { transcription },
        locked
      },
      stats {
        name,
        value
      }
    }
  }
}
      `;

export default withRouter(graphql(query)(HostsDetail));
