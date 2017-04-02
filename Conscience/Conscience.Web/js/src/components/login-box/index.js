import React from 'react';
import TextField from 'material-ui/TextField';
import RaisedButton from 'material-ui/RaisedButton';
import Relay from 'react-relay';

const style = {
  margin: 12,
};

export default class LoginBox extends React.Component {
    constructor(props) {
        super(props);
        
        this._doLogin = this._doLogin.bind(this);
        
        this.state = {
            userName: '',
            password: ''
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
                    <div>
                        <RaisedButton label="Login" primary={true} style={style} onClick={this._doLogin} />
                    </div>
                </div>;
    }

    _doLogin() {
        console.log("Click " + this.state.userName + " - " + this.state.password);
        
        //TODO: Change the server implementation to match Relay mutation specifications
        // this.props.relay.commitUpdate(
        //     new LoginMutation({userName: this.state.userName, passsword: this.state.password})
        // );
    }
}

// class LoginMutation extends Relay.Mutation
// {
//     getMutation() {
//         return Relay.QL`
//             mutation Login($userName: String!, $password: String!) {
//                 accounts
//                 {
//                     login(userName:$userName, password:$password)
//                     {
//                         id
//                     }
//                 }
//             }
//         `;
//     }

//     getVariables() {
//         return {userName: this.props.userName, password: this.props.password};
//     }
// }