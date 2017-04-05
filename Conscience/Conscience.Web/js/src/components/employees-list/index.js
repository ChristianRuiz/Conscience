import React from 'react';
import { graphql, gql } from 'react-apollo';
import { Table, TableBody, TableHeader, TableHeaderColumn, TableRow, TableRowColumn } from 'material-ui/Table';

class EmployeesList extends React.Component {
    render() {
        if (this.props.data.loading) {
            return (<div>Loading...</div>)
        }

        let employeeRows = this.props.data.employees.getAll.map(employee => {
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

const query = gql`query getEmployees {
            employees {
                getAll {
                        id,
                        account {
                            userName
                        },
                        department
                }
            }
        }
      `;

export default graphql(query)(EmployeesList);