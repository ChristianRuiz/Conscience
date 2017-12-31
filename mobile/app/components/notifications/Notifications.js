import React from 'react';

import {
  View,
  ScrollView,
  StyleSheet
} from 'react-native';

import { graphql, compose, gql } from 'react-apollo';
import Spinner from 'react-native-loading-spinner-overlay';

import Background from '../common/Background';
import Text from '../common/Text';
import Divider from '../common/Divider';
import commonStyles from '../../styles/common';

import query from '../../queries/HostDetailQuery';

const styles = StyleSheet.create({
  plotTitle: {
    marginTop: -20
  },
  detailsLine: {
    width: 260,
    justifyContent: 'space-between',
    flexDirection: 'row'
  },
  details: {
    width: 180
  }
});

class Notifications extends React.Component {
  render() {
    if (this.props.data.loading || !this.props.data.accounts || !this.props.data.accounts.current) {
      return (<View style={commonStyles.container}>
        <Background />
        <Spinner visible />
      </View>);
    }

    const notifications = this.props.data.notifications.current;

    const unreadNotifications = notifications.filter(n => !n.read).reverse();
    unreadNotifications.forEach((notification) => {
      if (notification.audio) {
        global.audioService.queueSound(notification.audio.path);
      }
    });

    if (unreadNotifications.length) {
      this.props.mutate({
        variables: { ids: unreadNotifications.map(n => n.id) }
      });
    }

    // TODO: A display template for notifications with audio and transcript

    return (<ScrollView>
      <Background />

      <View style={commonStyles.scrollBoxContainer}>
        <Background />
        {notifications && notifications.length ? (<View>
          {notifications.map(notification =>
            <View key={notification.id}>
              <Text>{notification.description}</Text>
              <Divider />
            </View>)}
        </View>) : <View><Text>You've got no notifications yet!</Text></View> }
      </View>
    </ScrollView>);
  }
}

Notifications.propTypes = {
  data: React.PropTypes.object.isRequired,
  mutate: React.PropTypes.func.isRequired
};

const mutation = gql`
mutation MarkNotificationsAsRead ($ids:[Int]) {
  notifications {
     markAsRead(ids: $ids) {
      id
      description
      timeStamp
      read
      notificationType
      employee {
        id
        name
        department
      }
      audio {
        id
        transcription
        path
      }
    }
  }
}
`;

export default compose(graphql(query),
                graphql(mutation))(Notifications);
