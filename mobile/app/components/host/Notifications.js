import React from 'react';

import {
  View,
  ScrollView
} from 'react-native';

import Background from '../common/Background';
import Text from '../common/Text';
import commonStyles from '../../styles/common';

class Notifications extends React.Component {
  render() {
    return (<ScrollView>
      <Background />

      <View style={commonStyles.scrollBoxContainer}>
        <Background />
        <Text>You've got no notifications yet!</Text>
      </View>
    </ScrollView>);
  }
}

export default Notifications;
