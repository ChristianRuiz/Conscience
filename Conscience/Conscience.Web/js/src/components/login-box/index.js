import React from 'react';
import { graphql, gql } from 'react-apollo';
import TextField from 'material-ui/TextField';
import RaisedButton from 'material-ui/RaisedButton';

const style = {
  margin: 12,
};

class LoginBox extends React.Component {
    constructor(props) {
        super(props);
        
        this._doLogin = this._doLogin.bind(this);
        
        this.state = {
            userName: '',
            password: '',
            hasError: false
        };
    }
    
    render() {
        return  <div>
                    <div>
                        <TextField 
                            hintText="User name"
                            onChange={ e => this.setState({userName: e.target.value})  }
                        />
                    </div>
                    <div>
                        <TextField
                            type="password"
                            hintText="Password"
                            onChange={ e => this.setState({password: e.target.value}) }
                        />
                    </div>
                    {this.state.hasError &&
                        <h2>Login error</h2>
                    }
                    <div>
                        <RaisedButton label="Login" primary={true} style={style} onClick={this._doLogin} />
                    </div>
                </div>;
    }

    _doLogin() {
        this.props.mutate({
            variables: { userName: this.state.userName, password: this.state.password }
        })
        .then(({ data }) => {
            document.location.href = "/Home";
        }).catch((error) => {
            this.setState({hasError: true});
        });
    }
}

const mutation = gql`
mutation Login($userName: String!, $password: String!) {
  accounts
  {
    login(userName:$userName, password:$password)
    {
      id
    }
  }
}
`;

export default graphql(mutation)(LoginBox);