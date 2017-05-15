import React from 'react';

import {
  StyleSheet,
  Text,
  View
} from 'react-native';
import { Button } from 'react-native-material-ui';

import { graphql, gql } from 'react-apollo';

import LogoutButton from './login/LogoutButton';

class Debugger extends React.Component {
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

        <Button raised text="Play 1" onPress={() => this.props.audioService.playSound('1.mp3')} />
        <Button raised text="Play 8" onPress={() => this.props.audioService.playSound('8.mp3')} />
        <Button raised text="Queue 1" onPress={() => this.props.audioService.queueSound('1.mp3')} />
        <Button raised text="Queue 8" onPress={() => this.props.audioService.queueSound('8.mp3')} />
        <Button raised text="Moderna" onPress={() => this.props.audioService.playSound('uploads/moderna.mp3')} />
        <LogoutButton />
      </View>
    );
  }
}

Debugger.propTypes = {
  data: React.PropTypes.object.isRequired,
  audioService: React.PropTypes.object.isRequired
};

const query = gql`
{
  accounts {
    current {
      id,
      device {
        id,
        deviceId,
        batteryLevel,
        batteryStatus,
        currentLocation {
          id,
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
