import React from 'react';
import ReactDOM from 'react-dom';
import Relay from 'react-relay'

import EmployeesList from '../components/employees-list'

class EmployeesRoute extends Relay.Route {
    static routeName = 'Employees';
    static queries = {
        employees: (Component) => Relay.QL`
        query EmployeesQuery {
            employees {
                ${Component.getFragment('employees')}
            }
        }
        `
    }
}

Relay.injectNetworkLayer(new Relay.DefaultNetworkLayer('/api/graphql', { credentials: 'same-origin' }));

ReactDOM.render(<Relay.RootContainer Component={EmployeesList}
                    route={new EmployeesRoute()}
                />, document.getElementById('main'));