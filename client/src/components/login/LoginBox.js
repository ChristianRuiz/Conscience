import React from 'react';
import { graphql, gql } from 'react-apollo';
import TextField from 'material-ui/TextField';
import RaisedButton from 'material-ui/RaisedButton';

import styles from '../../styles/components/login/login.css';

import Roles from '../../enums/roles';

const style = {
  margin: 12
};

class LoginBox extends React.Component {
  constructor(props) {
    super(props);

    this._doLogin = this._doLogin.bind(this);

    this.state = {
      userName: '', // 'arnold', // TODO: This values are just for development
      password: '', // '123456',
      hasError: false
    };
  }

  _doLogin() {
    this.props.mutate({
      variables: { userName: this.state.userName, password: this.state.password }
    })
        .then(({ data }) => {
          let redirectUrl = '/';
          if (document.location.search && document.location.search.indexOf('ReturnUrl') > 0) {
            redirectUrl = decodeURIComponent(document.location.search.substring(document.location.search.indexOf('=') + 1));
            if (document.location.hash) {
              redirectUrl += document.location.hash;
            }
          }

          if (redirectUrl === '/'
          && data.accounts.login.roles.length === 1
          && data.accounts.login.roles[0].name === Roles.Host) {
            redirectUrl += '#host';
          }

          document.location.href = redirectUrl;
        }).catch(() => {
          this.setState({ hasError: true });
        });
  }

  render() {
    return (<div className="loginPage">
      <div className="loginBoxDoubleBorder">
        <div className="loginBox">
          <div className="logoContainer">
            <img className="logo" alt="Aleph" src="/content/images/common/logo.png" />
          </div>
          <div>
            <TextField
              hintText="User name"
              value={this.state.userName}
              onChange={e => this.setState({ userName: e.target.value })}
            />
          </div>
          <div>
            <TextField
              type="password"
              hintText="Password"
              value={this.state.password}
              onChange={e => this.setState({ password: e.target.value })}
            />
          </div>
          {this.state.hasError &&
            <h2>Login error</h2>
                        }
          <div className="loginButtonContainer">
            <RaisedButton label="Login" primary style={style} onClick={this._doLogin} />
          </div>
        </div>
      </div>
    </div>);
  }
}

LoginBox.propTypes = {
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

export default graphql(mutation)(LoginBox);
