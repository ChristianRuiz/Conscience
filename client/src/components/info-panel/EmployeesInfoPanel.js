import React from 'react';
import { Link, withRouter } from 'react-router-dom';
import { graphql, gql } from 'react-apollo';

import AccountPicture from '../common/AccountPicture';
import ScrollableContainer from '../common/ScrollableContainer';
import RolesValidation from '../common/RolesValidation';
import Roles from '../../enums/roles';

import styles from '../../styles/components/info-panel/hostsInfoPanel.css';

class EmployeesInfoPanel extends React.Component {
  render() {
    if (this.props.data.loading) {
      return (<div>Loading...</div>);
    }

    const employee = this.props.data.employees.byId;

    if (employee !== null) {
      return (<div className="infoPanel">
        <ScrollableContainer>
          <div>
            <Link to={`/security-employee/${employee.id}`} ><AccountPicture pictureUrl={employee.account.pictureUrl} /></Link>
            <div className="cardEmployee">
              <Link to={`/security-employee/${employee.id}`} ><h1 className="charName">{employee.name}</h1></Link>

              <p className="department">{employee.department}</p>
            </div>
            <RolesValidation allowed={[Roles.Admin, Roles.CompanyAdmin, Roles.CompanyQA]}>
              <div>
                <Link to={`/security-employee/${employee.id}`} ><h3>Log</h3></Link>
              </div>
            </RolesValidation>
          </div>
        </ScrollableContainer>
      </div>);
    }

    return <div />;
  }
}

EmployeesInfoPanel.propTypes = {
  data: React.PropTypes.object.isRequired
};

const query = gql`query GetEmployeeDetails($employeeId:Int!) {
  employees {
    byId(id:$employeeId)
    {
      id,
      name,
      department,
      account {
        id,
        userName,
        pictureUrl,
        device {
          id,
          lastConnection,
          online,
          batteryLevel
        }
      }
    }
  }
}
      `;


export default withRouter(graphql(query)(EmployeesInfoPanel));
