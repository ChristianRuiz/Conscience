import React from 'react';
import { Link, withRouter } from 'react-router-dom';
import { graphql, gql } from 'react-apollo';
import ScrollableContainer from '../common/ScrollableContainer';

class BehaviourList extends React.Component {
  render() {
    if (this.props.data.loading) {
      return (<div>Loading...</div>);
    }

    return (<ScrollableContainer>
      <div>
        <h2>Behaviour</h2>
        <ul>
          {this.props.data.hosts.all.map(host =>
            <li key={host.id}>
              <p>
                <Link to={`/behaviour-detail/${host.id}`} ><b>{host.account.userName}: </b></Link>
                {host.currentCharacter ? host.currentCharacter.character.name : ''}
              </p>
            </li>)}
        </ul>
      </div>
    </ScrollableContainer>);
  }
}

BehaviourList.propTypes = {
  data: React.PropTypes.object.isRequired
};

const query = gql`query GetHosts {
            hosts {
              all {
                id,
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

export default withRouter(graphql(query)(BehaviourList));
