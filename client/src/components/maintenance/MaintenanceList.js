import React from 'react';
import { Link, withRouter } from 'react-router-dom';
import { graphql, gql } from 'react-apollo';
import ScrollableContainer from '../common/ScrollableContainer';

class MaintenanceList extends React.Component {
  getColor(host) {
    if (host.status === 'HURT') {
      return 'yellow';
    } else if (host.status === 'DEAD') {
      return 'red';
    }

    return '';
  }

  render() {
    if (this.props.data.loading) {
      return (<div>Loading...</div>);
    }

    const hostsWithCharacter = this.props.data.hosts.all.filter(h => h.currentCharacter);
    const hostsWithoutCharacter = this.props.data.hosts.all.filter(h => !h.currentCharacter);

    return (<ScrollableContainer>
      <div>
        <h2>Maintenance - Hosts</h2>
        <ul>
          {hostsWithCharacter.map(host =>
            <li key={host.id}>
              <p className={this.getColor(host)}>
                <Link to={`/maintenance-detail/${host.id}`} ><b>{host.account.userName}: </b></Link>
                {host.currentCharacter ? host.currentCharacter.character.name : ''}
                {` - ${host.status}`}
              </p>
            </li>)}
        </ul>
        { (hostsWithoutCharacter.length > 0) ?
        (<div>
          <h2>Unasigned Hosts</h2>
          <ul>
            {hostsWithoutCharacter.map(host =>
              <li key={host.id}>
                <p className={this.getColor(host)}>
                  <Link to={`/maintenance-detail/${host.id}`} ><b>{host.account.userName}: </b></Link>
                  {host.currentCharacter ? host.currentCharacter.character.name : ''}
                </p>
              </li>)}
          </ul>
        </div>) : '' }
      </div>
    </ScrollableContainer>);
  }
}

MaintenanceList.propTypes = {
  data: React.PropTypes.object.isRequired
};

const query = gql`query GetHosts {
  hosts {
    all {
      id,
      status
      account {
        id,
        userName
      },
      currentCharacter {
        id,
        assignedOn,
        character {
          id,
          name
        }
      }
    }
  }
}
      `;

export default withRouter(graphql(query, { options: { fetchPolicy: 'network-only' } })(MaintenanceList));
