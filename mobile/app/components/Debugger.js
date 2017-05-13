import React from 'react';

import {
  StyleSheet,
  Text,
  View
} from 'react-native';
import { Button } from 'react-native-material-ui';

import AudioService from '../services/AudioService';
import SignalRService from './services/SignalRService';

let audioService = null;

class Debugger extends React.Component {
  constructor(props) {
    super(props);

    this._signalRListener = this._signalRListener.bind(this);

    this.state = { batteryLevel: 0, charging: false };
  }

  componentDidMount() {
    audioService = new AudioService();
  }

  _signalRListener(changes) {
    this.setState(changes);
  }

  render() {
    let locationsComp = <View />;

    if (this.state.locations && this.state.locations.length > 0) {
      const currentLocation = this.state.locations[this.state.locations.length - 1];
      locationsComp = (<View>
        <Text>lat: {currentLocation.coords.latitude}</Text>
        <Text>long: {currentLocation.coords.longitude}</Text>
        <Text>time: {currentLocation.timestamp}</Text>
      </View>);
    }

    return (
      <View style={styles.container}>
        <Text>level: {this.state.batteryLevel}</Text>
        <Text>charging: {this.state.charging.toString()}</Text>

        {locationsComp}

        <Button raised text="1" onPress={() => audioService.playSound('1.mp3')} />
        <Button raised text="8" onPress={() => audioService.playSound('8.mp3')} />
        <Button raised text="Moderna" onPress={() => audioService.playSound('uploads/moderna.mp3')} />

        <SignalRService listener={this._signalRListener} />
      </View>
    );
  }
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center'
  }
});

export default Debugger;
