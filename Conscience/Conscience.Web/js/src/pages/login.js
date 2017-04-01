import React from 'react';
import ReactDOM from 'react-dom';
import Relay from 'react-relay'

import LoginBox from '../components/login-box'

/*class LoginRoute extends Relay.Route {
    static routeName = 'Login';
    static queries = {
        store: (Component) => Relay.QL`
        query MainQuery {
            employees {
                getAll
                {
                    ${Component.getFragment('getAll')}
                }
            }
        }
        `
    }
}

ReactDOM.render(<Relay.RootContainer Component={LoginBox}
                    Route={new LoginRoute()}
                />, document.getElementById('main'));*/

ReactDOM.render(<LoginBox/>, document.getElementById('main'));