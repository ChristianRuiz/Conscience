import React from 'react';
import TextField from 'material-ui/TextField';
import RaisedButton from 'material-ui/RaisedButton';
import Relay from 'graphql-relay';

const style = {
  margin: 12,
};

export default class LoginBox extends React.Component {
    render() {
        return <MuiThemeProvider>
                    <div>
                        <div>
                            <TextField 
                                hintText="User name"
                            />
                        </div>
                        <div>
                            <TextField
                                type="password"
                                hintText="Password"
                            />
                        </div>
                        <div>
                            <RaisedButton label="Login" primary={true} style={style} />
                        </div>
                    </div>
                </MuiThemeProvider>;
    }
}

