import React from 'react';

import {
  StyleSheet,
  Text,
  View
} from 'react-native';
import { Button } from 'react-native-material-ui';

import { graphql, gql } from 'react-apollo';

import AudioService from '../services/AudioService';

let audioService = null;

class Debugger extends React.Component {
  componentDidMount() {
    audioService = new AudioService();
  }

  render() {
    if (this.props.data.loading || !this.props.data.accounts) {
      return <View />;
    }

    const device = this.props.data.accounts.current.device;

    if (!device) {
      return <View style={styles.container}><Text>No device</Text></View>;
    }

    return (
      <View style={styles.container}>
        <Text>deviceId: {device.deviceId}</Text>
        <Text>level: {device.batteryLevel}</Text>
        <Text>charging: {device.batteryStatus}</Text>

        {device.currentLocation ? (<View>
          <Text>lat: {device.currentLocation.latitude}</Text>
          <Text>long: {device.currentLocation.longitude}</Text>
          <Text>time: {device.currentLocation.timeStamp}</Text>
        </View>) : <View /> }

        <Button raised text="1" onPress={() => audioService.playSound('1.mp3')} />
        <Button raised text="8" onPress={() => audioService.playSound('8.mp3')} />
        <Button raised text="Moderna" onPress={() => audioService.playSound('uploads/moderna.mp3')} />
      </View>
    );
  }
}

Debugger.propTypes = {
  data: React.PropTypes.object.isRequired
};

const query = gql`
{
  accounts {
    current {
      device {
        deviceId,
        batteryLevel,
        batteryStatus,
        currentLocation {
          latitude,
          longitude,
          timeStamp
        }
      }
    }
  }
}
`;

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center'
  }
});

export default graphql(query)(Debugger);
