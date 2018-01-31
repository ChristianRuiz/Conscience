import React from 'react';

import {
  View,
  ScrollView,
  StyleSheet
} from 'react-native';
import { Redirect } from 'react-router-native';

import { graphql } from 'react-apollo';
import * as Keychain from 'react-native-keychain';
import RNExitApp from 'react-native-exit-app';

import Background from '../common/Background';
import Text from '../common/Text';
import Button from '../common/Button';
import HostStateButtons from '../host/HostStateButtons';
import commonStyles from '../../styles/common';

import PanicButton from './PanicButton';

import Constants from '../../constants';
import query from '../../queries/HostDetailQuery';

const styles = StyleSheet.create({
  text: {
    fontSize: 18
  },
  button: {
    marginBottom: 20,
    width: 80
  },
  buttonsContainer: {
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
    marginTop: 100,
    fontSize: 8,
    color: 'gray'
  }
});

class HostButtons extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      logout: false
    };

    this.logout = this.logout.bind(this);
  }

  logout() {
    // Hack: Reseting the credentials two times to avoid Android chaching them.
    Keychain.resetGenericPassword()
    .then(() => {
      Keychain.resetGenericPassword()
      .then(() => {
        setTimeout(() => {
          RNExitApp.exitApp();
        }, 1000);
      });
    });
  }

  render() {
    if (this.state.logout) {
      return <Redirect to="/" />;
    }

    return (<ScrollView>
      <Background />

      <View style={[commonStyles.scrollBoxContainer, styles.buttonsContainer]}>
        {!this.props.data.loading && !this.props.data.accounts.current.employee ?
          (<HostStateButtons />) : <View />}

        <Text style={styles.textServer}>{Constants.SERVER_URL.replace('http://', '')} v1.2.5</Text>
        <Button title="Logout" onPress={() => this.logout()} />
      </View>
    </ScrollView>);
  }
}

HostButtons.propTypes = {
  data: React.PropTypes.object.isRequired
};

export default graphql(query)(HostButtons);
