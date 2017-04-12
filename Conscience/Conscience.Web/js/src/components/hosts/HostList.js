import React from 'react';
import { withRouter } from 'react-router-dom';
import { graphql, gql } from 'react-apollo';
import { Table, TableBody, TableHeader, TableHeaderColumn, TableRow, TableRowColumn } from 'material-ui/Table';

class HostsList extends React.Component {
  render() {
    if (this.props.data.loading) {
      return (<div>Loading...</div>);
    }

    const hostRows =
    this.props.data.hosts.getAll.map(host =>
      <TableRow key={host.id}>
        <TableRowColumn>{host.account.userName}</TableRowColumn>
      </TableRow>);

    return (<Table>
      <TableHeader>
        <TableRow>
          <TableHeaderColumn>Name</TableHeaderColumn>
        </TableRow>
      </TableHeader>
      <TableBody>
        { hostRows }
      </TableBody>
    </Table>);
  }
}

const query = gql`query GetHosts {
            hosts {
              getAll {
                id,
                account {
                  userName
                }
              }
            }
        }
      `;

export default withRouter(graphql(query)(HostsList));
