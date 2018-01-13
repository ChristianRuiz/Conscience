import React from 'react';

import {
  View,
  ScrollView,
  StyleSheet,
  Modal
} from 'react-native';

import { graphql, compose, gql } from 'react-apollo';
import Spinner from 'react-native-loading-spinner-overlay';

import Background from '../common/Background';
import Text from '../common/Text';
import Divider from '../common/Divider';
import Button from '../common/Button';
import commonStyles from '../../styles/common';

import query from '../../queries/HostDetailQuery';

const styles = StyleSheet.create({
  notificationsLine: {
    justifyContent: 'space-between',
    flexDirection: 'row',
    marginTop: 20
  },
  modal: {
    padding: 20,
    backgroundColor: '#194353'
  },
  transcriptText: {
    marginBottom: 30
  },
  coreMemotyTitle: {
    marginBottom: 10,
    color: '#F8BB3E'
  }
});

class Notifications extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      isModalDisplayed: false
    };

    this.getNotificationTemplate = this.getNotificationTemplate.bind(this);
    this.playAudioWithTranscript = this.playAudioWithTranscript.bind(this);
    this.stopAudio = this.stopAudio.bind(this);
  }

  componentWillReceiveProps(props) {
    const unreadNotifications = props.data.notifications.current.filter(n => !n.read).reverse();
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
  }

  getNotificationTemplate(notification) {
    if (notification.audio && notification.audio.transcription) {
      return (<View key={notification.id}>
        <View style={styles.notificationsLine}>
          <Text>{notification.description}</Text>
          <Button title="Play" onPress={() => this.playAudioWithTranscript(notification.audio)} />
        </View>
        <Divider />
      </View>);
    }

    return (<View key={notification.id}>
      <Text>{notification.description}</Text>
      <Divider />
    </View>);
  }

  playAudioWithTranscript(audio) {
    global.audioService.playSound(audio.path);
    this.setState({ isModalDisplayed: true, audio });
  }

  stopAudio() {
    this.setState({ isModalDisplayed: false });
    global.audioService.stopPlaying();
  }

  render() {
    if (this.props.data.loading || !this.props.data.accounts || !this.props.data.accounts.current) {
      return (<View style={commonStyles.container}>
        <Background />
        <Spinner visible />
      </View>);
    }

    const notifications = this.props.data.notifications.current;

    return (<ScrollView>
      <Background />

      <View style={commonStyles.scrollBoxContainer}>
        <Background />
        {notifications && notifications.length ? (<View>
          {notifications.map(notification =>
            this.getNotificationTemplate(notification))}
        </View>) : <View><Text>You've got no notifications yet!</Text></View> }
      </View>

      <Modal
        transparent
        visible={this.state.isModalDisplayed}
        onRequestClose={() => global.audioService.stopPlaying()}
      >
        <ScrollView style={styles.modal}>
          { this.props.data.accounts.current.host ? 
          (<View>
            <Text style={styles.coreMemotyTitle}>/***** CORE MEMORY ACCESS ****/</Text>
            <Text style={styles.coreMemotyTitle}>/* Do not share this with humans */</Text>
          </View>) : <View /> }
          <Text style={styles.transcriptText}>{this.state.audio ? this.state.audio.transcription.replace('\n', '\n\n') : ''}</Text>
          <Button title="Close" onPress={() => this.stopAudio()} />
        </ScrollView>
      </Modal>
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