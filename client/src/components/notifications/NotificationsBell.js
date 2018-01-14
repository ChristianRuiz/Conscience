import React from 'react';
import { Link } from 'react-router-dom';
import { gql, graphql, compose } from 'react-apollo';

import query from '../../queries/NotificationBell';

import styles from '../../styles/components/notifications/notifications.css';

class NotificationsBell extends React.Component {
  bellClick() {
    if (!this.props.data.loading && this.props.data.notifications) {
      const unreadNotificationsIds = this.props.data.notifications.current.filter(n => !n.read).map(n => n.id);
      if (unreadNotificationsIds.length) {
        this.props.mutate({ variables: { ids: unreadNotificationsIds } });
      }
    }

    if (window.panicAudio) {
      try {
        window.panicAudio.pause();
        window.panicAudio = null;
      } catch (e) { }
    }
  }

  render() {
    if (this.props.data.loading) {
      return (<div>Loading...</div>);
    }

    const unreadNotifications = this.props.data.notifications.current.filter(n => !n.read);

    return (<Link to="/notifications" onClick={() => this.bellClick()}>
      <img className="notificationsBell" alt="Notifications" src="/content/images/menu/nots.png" />
      {unreadNotifications.length ? <span className="notificationsBadge">{unreadNotifications.length}</span> : '' }
    </Link>);
  }
}

NotificationsBell.propTypes = {
  data: React.PropTypes.object.isRequired,
  mutate: React.PropTypes.func.isRequired
};

const mutation = gql`
mutation ReadNotifications($ids:[Int]) {
  notifications {
    markAsRead(ids:$ids) {
      id
      read
    }
  }
}
`;

export default compose(graphql(query), graphql(mutation))(NotificationsBell);
