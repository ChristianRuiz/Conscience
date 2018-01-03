import React from 'react';

import {
  View,
  StyleSheet
} from 'react-native';

import { graphql, compose, gql } from 'react-apollo';

import Text from '../common/Text';
import Button from '../buttons/Button';

import query from '../../queries/HostDetailQuery';

const styles = StyleSheet.create({
  text: {
    fontSize: 18
  },
  button: {
    marginBottom: 20,
    width: 80
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
  }
});

class HostStateButtons extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      changingStatus: false
    };

    this._stateChanged = this._stateChanged.bind(this);
  }

  _stateChanged(value) {
    this.setState({ status: value, changingStatus: true });
    this.props.mutate({
      variables: { status: value }
    }).then(() => {
      this.setState({ changingStatus: false });
    });
  }

  componentWillReceiveProps() {
    if (!this.props.data.loading &&
      !this.state.changingStatus &&
      this.state.status !== this.props.data.accounts.current.host.status) {
      this.setState({ status: this.props.data.accounts.current.host.status });
    }
  }

  render() {
    const underlayColor = '#2980B9';

    return (<View>
      <View style={styles.stateContainer}>
        <Text style={styles.stateText}>Status: {this.state.status ? this.state.status : 'OK' }</Text>
      </View>
      <View style={styles.stateButtonsContainer}>
        <Button
          style={styles.button}
          underlayColor={underlayColor}
          onPress={() => this._stateChanged('DEAD')}
        >
          <Text style={styles.text}>Dead</Text>
        </Button>
        <Button
          style={styles.button}
          underlayColor={underlayColor}
          onPress={() => this._stateChanged('HURT')}
        >
          <Text style={styles.text}>Hurt</Text>
        </Button>
        <Button
          style={styles.button}
          underlayColor={underlayColor}
          onPress={() => this._stateChanged('OK')}
        >
          <Text style={styles.text}>OK</Text>
        </Button>
      </View>
    </View>);
  }
}

HostStateButtons.propTypes = {
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
                graphql(mutation))(HostStateButtons);
