import React from 'react';
import { graphql, gql } from 'react-apollo';
import {
  StyleSheet,
  View
} from 'react-native';
import { Redirect } from 'react-router-native';
import Spinner from 'react-native-loading-spinner-overlay';
import * as Keychain from 'react-native-keychain';
import CheckBox from 'react-native-check-box'

import TextInput from '../common/TextInput';
import Text from '../common/Text';
import Button from '../common/Button';
import Background from '../common/Background';
import commonStyles from '../../styles/common';
import Constants from '../../constants';

const styles = StyleSheet.create({
  text: {
    width: 200,
    height: 50,
    marginBottom: 10
  },
  loginBox: {
    borderWidth: 1,
    borderColor: '#276077',
    padding: 10
  },
  loginBoxDoubleBorder: {
    borderWidth: 1,
    borderColor: '#276077',
    padding: 2
  },
  loginError: {
    marginBottom: 20
  }
});

class Login extends React.Component {
  constructor(props) {
    super(props);

    this._doLogin = this._doLogin.bind(this);

    this.state = {
      userName: '', // TODO: This values are just for development
      password: '123456',
      inLarp: false,
      hasError: false,
      loading: true,
      autoLogin: false
    };

    Keychain
      .getGenericPassword()
      .then((credentials) => {
        if (credentials) {
          this.setState({ userName: credentials.username.split('|')[0], password: credentials.password, inLarp: credentials.username.split('|')[1] === 'true', autoLogin: true });
          this._doLogin();
        } else {
          this.setState({ loading: false });
        }
      }).catch(() => {
        this.setState({ loading: false });
      });
  }

  _doLogin() {
    this.setState({ loading: true });

    Constants.changeServerUrl(this.state.inLarp ? 'http://192.168.1.100' : 'https://consciencelarp.azurewebsites.net');

    this.props.mutate({
      variables: { userName: this.state.userName, password: this.state.password }
    })
    .then((result) => {
      const user = `${this.state.userName}|${this.state.inLarp}`;
      Keychain.setGenericPassword(user, this.state.password);
      global.userName = this.state.userName;
      this.setState({ currentUser: result.data.accounts.login, loading: false });
    }).catch((error) => {
      console.log(`Unable to login ${JSON.stringify(error)}`);
      this.setState({ currentUser: null, hasError: true, loading: false, inLarp: this.state.autoLogin ? false : this.state.inLarp, autoLogin: false });
    });
  }

  render() {
    if (this.state.currentUser) {
      return <Redirect to="/tabs" />;
    }

    return (<View style={commonStyles.container}>

      <Background />

      <View style={styles.loginBoxDoubleBorder}>
        <View style={styles.loginBox}>
          <TextInput
            style={styles.text}
            placeholder="User name"
            placeholderTextColor="#CED0CA"
            value={this.state.userName}
            onChangeText={text => this.setState({ userName: text })}
          />
          <TextInput
            style={styles.text}
            type="password"
            placeholder="Password"
            placeholderTextColor="#CED0CA"
            secureTextEntry
            value={this.state.password}
            onChangeText={text => this.setState({ password: text })}
          />
          {this.state.hasError &&
          <Text style={styles.loginError}>Login error</Text>}

          <CheckBox
              rightTextStyle={StyleSheet.flatten(commonStyles.text)}
              onClick={() => this.setState({ inLarp: !this.state.inLarp })}
              isChecked={this.state.inLarp}
              rightText="Are you in the larp?"
              checkBoxColor="white"
          />

          <Button title="Login" onPress={this._doLogin} />
        </View>
      </View>

      <Spinner visible={this.state.loading} />
    </View>);
  }
}

Login.propTypes = {
  mutate: React.PropTypes.func.isRequired
};

const mutation = gql`
mutation Login($userName: String!, $password: String!) {
  accounts
  {
    login(userName:$userName, password:$password)
    {
      id,
      roles {
        name
      }
    }
  }
}
`;

export default graphql(mutation)(Login);
