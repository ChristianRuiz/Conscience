import React from 'react';

import {
  StyleSheet,
  Text,
  View
} from 'react-native';

class PlotEvents extends React.Component {
  render() {
    return (<View style={styles.container}>
      <Text>Plot events</Text>
    </View>);
  }
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center'
  }
});

export default PlotEvents;
