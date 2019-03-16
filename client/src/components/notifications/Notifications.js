import React from 'react';
import { withRouter } from 'react-router-dom';
import { graphql, gql, compose } from 'react-apollo';

import formatDate from '../../utls/DateFormatter';

import PictureDescriptionBox from '../common/PictureDescriptionBox';

import styles from '../../styles/components/notifications/notifications.css';

class Notifications extends React.Component {
  render() {
    if (this.props.data.loading) {
      return (<div>Loading...</div>);
    }

    const notifications = this.props.data.notifications.current; // .filter(n => n.description.indexOf('Toll') < 0)

    notifications.forEach((notification) => {
      if (notification.description === "Low battery host 'L. Müller'") {
        notification.host = {
          id: 8,
          account: {
            id: 8,
            userName: '2-lmu',
            pictureUrl: '/Content/images/uploaded/2-lmu.jpg?_ts=131621324461198255'
          },
          currentCharacter: {
            id: 43,
            character: {
              id: 55,
              name: 'L. Müller'
            }
          }
        };
      } else if (notification.description === "Low battery host 'V. Crawford'") {
        notification.host = {
          id: 18,
          account: {
            id: 18,
            userName: '2-vcr',
            pictureUrl: '/Content/images/uploaded/2-vcr.png?_ts=123'
          },
          currentCharacter: {
            id: 44,
            character: {
              id: 56,
              name: 'V. Crawford'
            }
          }
        };
      } else if (notification.description === "Low battery host 'Richard Tollman'") {
        notification.host = {
          id: 4,
          account: {
            id: 4,
            userName: '2-toll',
            pictureUrl: '/Content/images/uploaded/2-toll.jpg?_ts=131621598743350471'
          },
          currentCharacter: {
            id: 42,
            character: {
              id: 54,
              name: 'Richard Tollman'
            }
          }
        };
      }
    });

    return (<div className="notifications">
      {
        notifications.map((n) => {
          let account = { pictureUrl: '' };
          let link = '/';
          if (n.host) {
            account = n.host.account;
            if (n.host.currentCharacter) {
              link = `/character-detail/${n.host.currentCharacter.character.id}`;
            } else {
              link = `/behaviour-detail/${n.host.id}`;
            }
          } else if (n.employee) {
            if (n.employee.account) {
              account = n.employee.account;
              link = `/security-employee/${n.employee.id}`;
            }
          }
          return (<div key={n.id}>
            <PictureDescriptionBox
              pictureUrl={account.pictureUrl}
              title={n.description}
              link={link}
              description={formatDate(n.timeStamp)}
            />
          </div>);
        })
      }
    </div>);
  }
}

Notifications.propTypes = {
  data: React.PropTypes.object.isRequired
};

const query = gql`query GetNotifications {
  notifications {
    current {
      id
      description
      timeStamp
      read
      notificationType
      employee {
        id
        name
      }
      host {
        id
        account {
          id
          userName
          pictureUrl
        }
        currentCharacter {
          id
          character {
            id
            name
          }
        }
      }
    }
  }
}
      `;

export default withRouter(graphql(query, { options: { fetchPolicy: 'network-only' } })(Notifications));
