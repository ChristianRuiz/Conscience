import React from 'react';
import { graphql, gql, compose } from 'react-apollo';
import RaisedButton from 'material-ui/RaisedButton';

class LogoutButton extends React.Component {
  constructor(props) {
    super(props);
    this._doLogout = this._doLogout.bind(this);
  }

  _doLogout() {
    this.props.mutate()
        .then(({ data }) => {
          document.location.href = '/Login';
        }).catch(() => {
          console.warn('Unable to logout');
        });
  }

  render() {
    return (<div>{!this.props.data.loading && <label>{this.props.data.accounts.getCurrent.userName}</label>}
      <RaisedButton label="Logout" primary onClick={this._doLogout} /></div>);
  }
}

LogoutButton.propTypes = {
  data: React.PropTypes.object.isRequired,
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

const query = gql`
query GetCurrentUser {
  accounts
  {
    getCurrent
    {
      userName
    }
  }
}
`;

export default compose(graphql(mutation),
                        graphql(query))(LogoutButton);
