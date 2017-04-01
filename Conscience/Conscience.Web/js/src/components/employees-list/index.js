import React from 'react';
import Relay from 'react-relay';

import MuiThemeProvider from 'material-ui/styles/MuiThemeProvider';
import Table from 'material-ui/Table';

class EmployeesList extends React.Component {
    render() {
        let employeeRows = this.props.employees.getAll.map(employee => {
            return <li key={employee.id}>{employee.account.userName}</li>
        });

        return <ul>
                    { employeeRows }
                </ul>;
    }
}

EmployeesList = Relay.createContainer(EmployeesList, {
    fragments: {
        employees: () => Relay.QL`
            fragment on EmployeeQuery {
                getAll {
                    id,
                    account {
                        userName
                    }
                }
            }
        `
    }
});

export default EmployeesList;