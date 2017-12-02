import React from 'react';
import { Link, withRouter } from 'react-router-dom';
import { graphql, gql } from 'react-apollo';
import ScrollableContainer from '../common/ScrollableContainer';

class EmployeesList extends React.Component {
  render() {
    if (this.props.data.loading) {
      return (<div>Loading...</div>);
    }

    return (<ScrollableContainer>
      <div>
        <h2>Security</h2>
        <ul>
          {this.props.data.employees.all.map(employee =>
            <li key={employee.id}>
              <p>
                <Link to={`/security-detail/${employee.id}`} ><b>{employee.name} ({employee.department})</b></Link>
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

const query = gql`query GetEmployees {
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
        }
      `;

export default withRouter(graphql(query)(EmployeesList));
