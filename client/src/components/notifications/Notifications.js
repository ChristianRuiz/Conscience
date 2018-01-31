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

    return (<div className="notifications">
      {
        this.props.data.notifications.current.filter(n => n.description.indexOf('Toll') < 0 && n.description.indexOf('Crawford') < 0).map((n) => {
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

export default withRouter(graphql(query)(Notifications));
