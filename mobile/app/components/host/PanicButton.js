import React from 'react';

import {
  View,
  ScrollView,
  StyleSheet,
  Alert
} from 'react-native';
import { Redirect } from 'react-router-native';

import { graphql, compose, gql } from 'react-apollo';
import * as Keychain from 'react-native-keychain';

import Background from '../common/Background';
import Text from '../common/Text';
import Button from '../common/Button';
import HostButton from './HostButton';
import commonStyles from '../../styles/common';

import Constants from '../../constants';
import query from '../../queries/HostDetailQuery';

const styles = StyleSheet.create({
  textPanic: {
    fontSize: 22
  },
  panicButton: {
    height: 160
  }
});

class PanicButton extends React.Component {
  panicButton() {
    Alert.alert(
      'Panic!',
      'Are you sure you need assistance from the safety team?',
      [
        { text: 'No', style: 'cancel' },
        { text: 'Yes',
          onPress: () => {
            this.props.mutate().then(() => {
              Alert.alert(
                'Panic!',
                'Reported. The safety team will look for you. If you see any organizers talk to them directly.');
            });
          }
        }
      ],
      { cancelable: false }
    );
  }

  render() {
    const underlayColor = '#2980B9';

    return (<HostButton
      style={styles.panicButton}
      onPress={() => this.panicButton()}
      underlayColor={underlayColor}
    >
      <Text style={styles.textPanic} underlayColor={underlayColor}>Panic</Text>
    </HostButton>);
  }
}

PanicButton.propTypes = {
  mutate: React.PropTypes.func.isRequired
};

const mutation = gql`
mutation PanicButton {
  accounts {
    panic{
      id
    }
  }
}
`;

export default graphql(mutation)(PanicButton);
