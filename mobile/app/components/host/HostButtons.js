import React from 'react';

import {
  View,
  ScrollView,
  StyleSheet
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
      logout: false
    };

    this._stateChanged = this._stateChanged.bind(this);
    this.logout = this.logout.bind(this);
  }

  _stateChanged(value) {
    this.setState({ status: value, changingStatus: true });
    this.props.mutate({
      variables: { status: value }
    }).then(() => {
      this.setState({ changingStatus: false });
    });
  }

  logout() {
    Keychain
    .resetGenericPassword()
    .then(() => this.setState({ logout: true }));
  }

  componentWillReceiveProps() {
    if (!this.props.data.loading &&
      !this.state.changingStatus &&
      this.state.status !== this.props.data.accounts.current.host.status) {
      this.setState({ status: this.props.data.accounts.current.host.status });
    }
  }

  render() {
    if (this.state.logout) {
      return <Redirect to="/" />;
    }

    const underlayColor = '#2980B9';

    return (<ScrollView>
      <Background />

      <View style={[commonStyles.scrollBoxContainer, styles.buttonsContainer]}>
        <View>
          <View style={styles.stateContainer}>
            <Text style={styles.stateText}>Status: {this.state.status ? this.state.status : 'OK' }</Text>
          </View>
          <View style={styles.stateButtonsContainer}>
            <HostButton
              style={styles.button}
              underlayColor={underlayColor}
              onPress={() => this._stateChanged('DEAD')}
            >
              <Text style={styles.text}>Dead</Text>
            </HostButton>
            <HostButton
              style={styles.button}
              underlayColor={underlayColor}
              onPress={() => this._stateChanged('HURT')}
            >
              <Text style={styles.text}>Hurt</Text>
            </HostButton>
            <HostButton
              style={styles.button}
              underlayColor={underlayColor}
              onPress={() => this._stateChanged('OK')}
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
        <Button title="Logout" onPress={() => this.logout()} />
      </View>
    </ScrollView>);
  }
}

HostButtons.propTypes = {
  data: React.PropTypes.object.isRequired,
  mutate: React.PropTypes.func.isRequired
};

const mutation = gql`
mutation ChangeHostStatus($status:HostStatus) {
  hosts {
    changeStatus(status:$status) {
      id
      status
    }
  }
}
`;

export default compose(graphql(query),
                graphql(mutation))(HostButtons);
