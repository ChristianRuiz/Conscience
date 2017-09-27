import React from 'react';
import { withRouter } from 'react-router-dom';
import { graphql, gql } from 'react-apollo';

class EmployeeDetail extends React.Component {
  render() {
    if (this.props.data.loading) {
      return (<div>Loading...</div>);
    }

    const employee = this.props.data.employees.byId;

    return (<div>
      <h1>{employee.name}</h1>
    </div>);
  }
}

EmployeeDetail.propTypes = {
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

export default withRouter(graphql(query)(EmployeeDetail));
