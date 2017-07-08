import React from 'react';
import { graphql, gql, withApollo } from 'react-apollo';
import {
  StyleSheet,
  View,
  Text
} from 'react-native';
import { Redirect } from 'react-router-native';

import TextInput from '../common/TextInput';
import Button from '../common/Button';
import Background from '../common/Background';
import commonStyles from '../../styles/common';

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
  }
});

class Login extends React.Component {
  constructor(props) {
    super(props);

    this._doLogin = this._doLogin.bind(this);

    this.state = {
      userName: 'dolores', // TODO: This values are just for development
      password: '123456',
      hasError: false
    };
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
    this.props.mutate({
      variables: { userName: this.state.userName, password: this.state.password }
    })
    .then((result) => {
      this.setState({ currentUser: result.data.accounts.login });
    }).catch((error) => {
      console.log(`Unable to login ${JSON.stringify(error)}`);
      this.state.hasError = true;
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
            value={this.state.userName}
            onChangeText={text => this.setState({ userName: text })}
          />
          <TextInput
            style={styles.text}
            type="password"
            placeholder="Password"
            secureTextEntry
            value={this.state.password}
            onChangeText={text => this.setState({ password: text })}
          />
          {this.state.hasError &&
          <Text style={commonStyles.h3}>Login error</Text>
                            }
          <Button title="Login" onPress={this._doLogin} />
        </View>
      </View>
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
