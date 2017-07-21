import React from 'react';

import {
  View,
  ScrollView,
  StyleSheet,
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
    marginBottom: 20,
    width: 80
  },
  panicButton: {
    height: 160
  },
  buttonsContainer: {
    justifyContent: 'space-between'
  },
  stateContainer: {
    justifyContent: 'center',
    alignItems: 'center'
  },
  stateButtonsContainer: {
    flexDirection: 'row',
    justifyContent: 'space-between'
  },
  stateText: {
    fontSize: 26,
    marginTop: 20,
    marginBottom: 60
  },
  textServer: {
    fontSize: 8,
    color: 'gray'
  }
});

class HostButtons extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      stateValue: 2
    };
  }

  _stateChanged(value) {
    this.setState({ stateValue: value });
  }

  render() {
    const underlayColor = '#2980B9';

    let status = 'OK';

    if (this.state.stateValue === 1) {
      status = 'HURT';
    } else if (this.state.stateValue === 0) {
      status = 'DEAD';
    }

    return (<ScrollView>
      <Background />

      <View style={[commonStyles.scrollBoxContainer, styles.buttonsContainer]}>
        <View>
          <View style={styles.stateContainer}>
            <Text style={styles.stateText}>Status: {status}</Text>
          </View>
          <View style={styles.stateButtonsContainer}>
            <HostButton
              style={styles.button}
              underlayColor={underlayColor}
              onPress={() => this._stateChanged(0)}
            >
              <Text style={styles.text}>Dead</Text>
            </HostButton>
            <HostButton
              style={styles.button}
              underlayColor={underlayColor}
              onPress={() => this._stateChanged(1)}
            >
              <Text style={styles.text}>Hurt</Text>
            </HostButton>
            <HostButton
              style={styles.button}
              underlayColor={underlayColor}
              onPress={() => this._stateChanged(2)}
            >
              <Text style={styles.text}>OK</Text>
            </HostButton>
          </View>
        </View>
        <HostButton
          style={styles.panicButton}
          onPress={() => alert('Sorry, not implemented yet. Look for an organizer.')}
          underlayColor={underlayColor}
        >
          <Text style={styles.textPanic} underlayColor={underlayColor}>Panic</Text>
        </HostButton>

        <Text style={styles.textServer}>{Constants.SERVER_URL.replace('http://', '')}</Text>
      </View>
    </ScrollView>);
  }
}

export default HostButtons;
