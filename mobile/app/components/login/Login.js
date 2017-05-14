import React from 'react';
import { graphql, gql, withApollo } from 'react-apollo';
import {
  StyleSheet,
  TextInput,
  View
} from 'react-native';
import { Redirect } from 'react-router-native';
import { Button } from 'react-native-material-ui';

import query from '../../queries/HostDetailQuery';

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

    return (<View style={styles.container}>
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
      <h3>Login error</h3>
                        }
      <Button raised text="Login" onPress={this._doLogin} />
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

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center'
  },
  text: {
    width: 200
  }
});

export default withApollo(graphql(mutation)(Login));
