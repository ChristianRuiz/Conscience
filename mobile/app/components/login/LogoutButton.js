import React from 'react';
import { graphql, gql, compose } from 'react-apollo';
import { Button } from 'react-native-material-ui';
import { Redirect } from 'react-router-native';

class LogoutButton extends React.Component {
  constructor(props) {
    super(props);
    this._doLogout = this._doLogout.bind(this);

    this.state = {
      logout: false
    };
  }

  _doLogout() {
    this.props.mutate()
        .then(() => {
          this.setState({ logout: true });
        }).catch(() => {
          console.log('Unable to logout');
        });
  }

  render() {
    if (this.state.logout) {
      return <Redirect to="/" />;
    }

    return <Button raised text="Logout" onPress={this._doLogout} />;
  }
}

LogoutButton.propTypes = {
  mutate: React.PropTypes.func.isRequired
};

const mutation = gql`
mutation Logout {
  accounts
  {
    logout
    {
      id
    }
  }
}
`;

export default graphql(mutation)(LogoutButton);
