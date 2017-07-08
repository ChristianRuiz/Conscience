import React from 'react';
import {
  StyleSheet,
  View
} from 'react-native';

import commonStyles from '../../styles/common';

const styles = StyleSheet.create({
  background: {
    position: 'absolute',
    backgroundColor: 'pink',
    top: 0,
    left: 0,
    bottom: 0,
    right: 0
  }
});

class Background extends React.Component {
  render() {
    return <View style={styles.background} />;
  }
}

export default Background;
