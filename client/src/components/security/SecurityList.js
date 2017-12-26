import React from 'react';
import { Link, withRouter } from 'react-router-dom';
import { graphql, gql } from 'react-apollo';
import ScrollableContainer from '../common/ScrollableContainer';

import RolesValidation from '../common/RolesValidation';
import Roles from '../../enums/roles';

class EmployeesList extends React.Component {
  render() {
    if (this.props.data.loading) {
      return (<div>Loading...</div>);
    }

    return (<ScrollableContainer>
      <div>
        <RolesValidation allowed={[Roles.Admin, Roles.CompanyAdmin, Roles.CompanyQA]}>
          <div>
            <h2>Security - Employees</h2>
            <ul>
              {this.props.data.employees.all.map(employee =>
                <li key={employee.id}>
                  <p>
                    <Link to={`/security-employee/${employee.id}`} ><b>{employee.name} ({employee.department})</b></Link>
                  </p>
                </li>)}
            </ul>
          </div>
        </RolesValidation>

        <h2>Security - Hosts</h2>
        <ul>
          {this.props.data.hosts.all.map(host =>
            <li key={host.id}>
              <p>
                <Link to={`/security-host/${host.id}`} ><b>{host.account.userName}: </b></Link>
                {host.currentCharacter ? host.currentCharacter.character.name : ''}
              </p>
            </li>)}
        </ul>
      </div>
    </ScrollableContainer>);
  }
}

EmployeesList.propTypes = {
  data: React.PropTypes.object.isRequired
};

const query = gql`query GetEmployeesAndHosts {
            employees {
              all {
                id,
                name,
                account {
                    id,
                    userName
                },
                department
              }
            }
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
                }
              }
            }
        }
      `;

export default withRouter(graphql(query)(EmployeesList));
