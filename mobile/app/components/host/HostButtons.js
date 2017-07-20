import React from 'react';

import {
  View,
  ScrollView,
  StyleSheet,
  Image,
  Slider
} from 'react-native';

import Background from '../common/Background';
import Text from '../common/Text';
import HostButton from './HostButton';
import commonStyles from '../../styles/common';

import Constants from '../../constants';

const styles = StyleSheet.create({
  text: {
    fontSize: 18
  },
  textPanic: {
    fontSize: 22
  },
  button: {
    marginBottom: 20
  },
  panicButton: {
    height: 160,
    marginBottom: 120
  },
  buttonsContainer: {
    justifyContent: 'space-between'
  },
  stateImage: {
    justifyContent: 'center',
    alignItems: 'center'
  },
  stateSlider: {
    marginTop: 40
  }
});

class HostButtons extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      sliderValue: 2
    };
  }

  _sliderChanged(value) {

  }

  render() {
    const underlayColor = '#2980B9';

    let source = require('../../img/states/fine.png');

    if (this.state.sliderValue === 1) {
      source = require('../../img/states/hurt.png');
    } else if (this.state.sliderValue === 0) {
      source = require('../../img/states/dead.png');
    }

    return (<ScrollView>
      <Background />

      <View style={[commonStyles.scrollBoxContainer, styles.buttonsContainer]}>
        <View>
          <View style={styles.stateImage}>
            <Image source={source} style={[commonStyles.image, styles.stateImage]} />
          </View>
          <Slider
            value={this.state.sliderValue} minimumValue={0} maximumValue={2} step={1}
            onValueChange={value => this.setState({ sliderValue: value })}
            onSlidingComplete={this._sliderChanged}
            style={styles.stateSlider}
          />
        </View>
        <HostButton style={styles.panicButton} onPress={() => alert('Sorry, not implemented yet. Find an organizer.')}>
          <Text style={styles.textPanic} underlayColor={underlayColor}>Panic</Text>
        </HostButton>
      </View>
    </ScrollView>);
  }
}

export default HostButtons;
