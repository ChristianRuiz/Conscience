import React from 'react';
import { withRouter } from 'react-router-dom';
import { graphql, gql } from 'react-apollo';

import LogConsole from './LogConsole';
import formatDate from '../../utls/DateFormatter';

class HostDetail extends React.Component {
  render() {
    if (this.props.data.loading) {
      return (<div>Loading...</div>);
    }

    const logs = this.props.data.logEntries.byHost;

    return (<div>
      <LogConsole title={`${this.props.data.hosts.byId.account.userName} log`} logEntries={logs} extraMargin={100} />

      { this.props.data.hosts.byId.characters ?
      (<ul>
        {this.props.data.hosts.byId.characters.map(c =>
          <li key={c.id}>
            <p>
              <b>{c.character.name}</b> from {formatDate(c.assignedOn)} {c.unassignedOn ? ` to ${formatDate(c.unassignedOn)}` : ''} : { c.character.narrativeFunction}
            </p>
          </li>
        )}
      </ul>) : '' }
    </div>);
  }
}

HostDetail.propTypes = {
  data: React.PropTypes.object.isRequired
};

const query = gql`query GetHostLogEntries($hostId:Int!) {
  logEntries {
    byHost(id: $hostId) {
      id
      description
      timeStamp
      host {
        id
        account {
          id
          userName
        }
        currentCharacter {
          id
          character {
            id
            name
          }
        }
      }
      employee {
        id
        name
      }
    }
  }
  hosts {
    byId(id: $hostId) {  
      id
      account {
        id
        userName
      }
      characters {
        id
        assignedOn
        unassignedOn
        character {
          id
          name
          narrativeFunction
        }
      }
    }
  }
}
      `;

export default withRouter(graphql(query)(HostDetail));
