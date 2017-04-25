import React from 'react';
import { withRouter } from 'react-router-dom';
import { graphql, gql } from 'react-apollo';
import { Table, TableBody, TableHeader, TableHeaderColumn, TableRow, TableRowColumn } from 'material-ui/Table';

class EmployeesList extends React.Component {
  render() {
    if (this.props.data.loading) {
      return (<div>Loading...</div>);
    }

    const employeeRows =
    this.props.data.employees.all.map(employee =>
      <TableRow key={employee.id}>
        <TableRowColumn>{employee.account.userName}</TableRowColumn>
        <TableRowColumn>{employee.department}</TableRowColumn>
      </TableRow>);

    return (<Table>
      <TableHeader>
        <TableRow>
          <TableHeaderColumn>Name</TableHeaderColumn>
          <TableHeaderColumn>Department</TableHeaderColumn>
        </TableRow>
      </TableHeader>
      <TableBody>
        { employeeRows }
      </TableBody>
    </Table>);
  }
}

EmployeesList.propTypes = {
  data: React.PropTypes.object.isRequired
};

const query = gql`query GetEmployees {
            employees {
                all {
                        id,
                        account {
                            userName
                        },
                        department
                }
            }
        }
      `;

export default withRouter(graphql(query)(EmployeesList));
