import React from 'react';
import { graphql, gql, withApollo } from 'react-apollo';
import {
  StyleSheet,
  View,
  Platform
} from 'react-native';
import { Redirect } from 'react-router-native';
import Spinner from 'react-native-loading-spinner-overlay';
import * as Keychain from 'react-native-keychain';

import TextInput from '../common/TextInput';
import Text from '../common/Text';
import Button from '../common/Button';
import Background from '../common/Background';
import commonStyles from '../../styles/common';
import Constants from '../../constants';

import query from '../../queries/HostDetailQuery';

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
      hasError: false,
      loading: true
    };

    Keychain
      .getGenericPassword()
      .then((credentials) => {
        if (credentials) {
          this.setState({ userName: credentials.username, password: credentials.password });
          this._doLogin();
        } else {
          this.setState({ loading: false, currentUser: false });
        }
      }).catch(() => {
        this.setState({ loading: false, currentUser: false });
      });
  }

  componentDidMount() {
    let data;

    try {
      data = this.props.client.readQuery({ query });
    } catch (e) {
      console.log('There is no current account on the cache');
      return;
    }

    this.state.currentUser = data.accounts.current;
  }

  _doLogin() {
    this.setState({ loading: true });

    if (!Constants.serverUrlInitialized) {
      setTimeout(this._doLogin, 1000);
      return;
    }

    this.props.mutate({
      variables: { userName: this.state.userName, password: this.state.password }
    })
    .then((result) => {
      Keychain.setGenericPassword(this.state.userName, this.state.password);
      this.setState({ currentUser: result.data.accounts.login, loading: false });
    }).catch((error) => {
      console.log(`Unable to login ${JSON.stringify(error)}`);
      this.setState({ currentUser: null, hasError: true, loading: false });
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
          <Button title="Login" onPress={this._doLogin} />
        </View>
      </View>

      <Spinner visible={this.state.loading} />
    </View>);
  }
}

Login.propTypes = {
  client: React.PropTypes.object.isRequired,
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

export default withApollo(graphql(mutation)(Login));
