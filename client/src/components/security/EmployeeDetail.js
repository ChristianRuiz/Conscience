import React from 'react';
import { withRouter } from 'react-router-dom';
import { graphql, gql } from 'react-apollo';

import LogConsole from './LogConsole';

class EmployeeDetail extends React.Component {
  render() {
    if (this.props.data.loading) {
      return (<div>Loading...</div>);
    }

    const logs = this.props.data.logEntries.byEmployee;

    return (<LogConsole title={`${this.props.data.employees.byId.name} log`} logEntries={logs} />);
  }
}

EmployeeDetail.propTypes = {
  data: React.PropTypes.object.isRequired
};

const query = gql`query GetEmployeeLogEntries($employeeId:Int!) {
  logEntries {
    byEmployee(id: $employeeId) {
      id
      description
      timeStamp
      host {
        id
        account {
          id
          userName
        }
        currentCharacter {
          id
          character {
            id
            name
          }
        }
      }
      employee {
        id
        name
      }
    }
  }
  employees {
    byId(id: $employeeId){
      id
      name
    }
  }
}
      `;

export default withRouter(graphql(query)(EmployeeDetail));
