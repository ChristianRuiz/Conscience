import React from 'react';
import { Link, withRouter } from 'react-router-dom';
import { graphql, gql } from 'react-apollo';
import { Table, TableBody, TableHeader, TableHeaderColumn, TableRow, TableRowColumn } from 'material-ui/Table';
import RolesValidation from '../common/RolesValidation';
import Roles from '../../enums/roles';

class HostsList extends React.Component {
  getMemoryComponent(memory) {
    let result = <a href='#'>Add</a>;

    if (memory) {
      if (memory.locked) {
        result = 'Locked';
      } else {
        result = 'Unlocked';
      }
    }

    return result;
  }

  render() {
    if (this.props.data.loading) {
      return (<div>Loading...</div>);
    }

    const hostRows =
    this.props.data.hosts.all.map(host =>
      <TableRow key={host.id}>
        <TableRowColumn>
           <Link to={`/host-detail/${host.id}`} >{host.account.userName}</Link>
        </TableRowColumn>
        <TableRowColumn>{host.currentCharacter ? host.currentCharacter.character.name : ''}</TableRowColumn>
        <RolesValidation allowed={[Roles.Admin]}>
          <TableRowColumn>{this.getMemoryComponent(host.coreMemory1)}</TableRowColumn>
        </RolesValidation>
        <RolesValidation allowed={[Roles.Admin]}>
          <TableRowColumn>{this.getMemoryComponent(host.coreMemory2)}</TableRowColumn>
        </RolesValidation>
        <RolesValidation allowed={[Roles.Admin]}>
          <TableRowColumn>{this.getMemoryComponent(host.coreMemory3)}</TableRowColumn>
        </RolesValidation>
      </TableRow>);

    return (<Table>
      <TableHeader>
        <TableRow>
          <TableHeaderColumn>Name</TableHeaderColumn>
          <TableHeaderColumn>Character</TableHeaderColumn>
          <RolesValidation allowed={[Roles.Admin]}>
            <TableHeaderColumn>Core Memory 1</TableHeaderColumn>
          </RolesValidation>
          <RolesValidation allowed={[Roles.Admin]}>
            <TableHeaderColumn>Core Memory 2</TableHeaderColumn>
          </RolesValidation>
          <RolesValidation allowed={[Roles.Admin]}>
            <TableHeaderColumn>Core Memory 3</TableHeaderColumn>
          </RolesValidation>
        </TableRow>
      </TableHeader>
      <TableBody>
        { hostRows }
      </TableBody>
    </Table>);
  }
}

HostsList.propTypes = {
  data: React.PropTypes.object.isRequired
};

const query = gql`query GetHosts {
            hosts {
              all {
                id,
                account {
                  id,
                  userName
                },
                currentCharacter {
                  id,
                  assignedOn,
                  character {
                    id,
                    name
                  }
                },
                coreMemory1 {
                  id,
                   locked
                },
                coreMemory2 {
                  id,
                   locked
                },
                coreMemory3 {
                  id,
                   locked
                }
              }
            }
        }
      `;

export default withRouter(graphql(query)(HostsList));
