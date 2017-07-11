import React from 'react';

import {
  View,
  ScrollView,
    StyleSheet
} from 'react-native';

import Background from '../common/Background';
import Text from '../common/Text';
import HostButton from './HostButton';
import commonStyles from '../../styles/common';

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
    marginBottom: 60
  },
  buttonsContainer: {
    justifyContent: 'space-between'
  }
});

class HostButtons extends React.Component {
  render() {
    const underlayColor = '#2980B9';

    return (<ScrollView>
      <Background />

      <View style={[commonStyles.scrollBoxContainer, styles.buttonsContainer]}>
        <Background />
        <View>
          <HostButton style={styles.button} underlayColor={underlayColor} onPress={() => alert('OK')}>
            <Text style={styles.text}>OK</Text>
          </HostButton>
          <HostButton style={styles.button}>
            <Text style={styles.text} underlayColor={underlayColor}>Hurt</Text>
          </HostButton>
          <HostButton style={styles.button}>
            <Text style={styles.text} underlayColor={underlayColor}>Dead</Text>
          </HostButton>
        </View>
        <HostButton style={styles.panicButton}>
          <Text style={styles.textPanic} underlayColor={underlayColor}>Panic</Text>
        </HostButton>
      </View>
    </ScrollView>);
  }
}

export default HostButtons;
