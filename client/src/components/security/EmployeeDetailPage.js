import React from 'react';
import EmployeeDetail from './EmployeeDetail';
import ScrollableContainer from '../common/ScrollableContainer';
import EmployeesInfoPanel from '../info-panel/EmployeesInfoPanel';

const HostDetailPage = ({ match }) =>
  <div className="mainContainer">
    <ScrollableContainer>
      <EmployeeDetail employeeId={match.params.employeeId} />
    </ScrollableContainer>
    <EmployeesInfoPanel employeeId={match.params.employeeId} />
  </div>;

HostDetailPage.propTypes = {
  match: React.PropTypes.object.isRequired
};

export default HostDetailPage;
