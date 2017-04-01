import React from 'react';
import Relay from 'react-relay';

import MuiThemeProvider from 'material-ui/styles/MuiThemeProvider';
import { Table, TableBody, TableHeader, TableHeaderColumn, TableRow, TableRowColumn } from 'material-ui/Table';

class EmployeesList extends React.Component {
    render() {
        let employeeRows = this.props.employees.getAll.map(employee => {
            return <TableRow key={employee.id}>
                        <TableRowColumn>{employee.account.userName}</TableRowColumn>
                        <TableRowColumn>{employee.department}</TableRowColumn>
                    </TableRow>
        });

        return <Table>
                    <TableHeader>
                        <TableRow>
                            <TableHeaderColumn>Name</TableHeaderColumn>
                            <TableHeaderColumn>Department</TableHeaderColumn>
                        </TableRow>
                    </TableHeader>
                    <TableBody>
                        { employeeRows }
                    </TableBody>
                </Table>;
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
                    },
                    department
                }
            }
        `
    }
});

export default EmployeesList;