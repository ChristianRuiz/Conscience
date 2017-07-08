import React from 'react';

import {
  Text,
  View
} from 'react-native';

import Background from '../background/Background';
import commonStyles from '../../styles/common';

class Notifications extends React.Component {
  render() {
    return (<View style={commonStyles.container}>
      <Background />
      <Text>You've got no notifications yet!</Text>
    </View>);
  }
}

export default Notifications;
