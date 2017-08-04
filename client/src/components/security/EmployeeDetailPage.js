import React from 'react';

const EmployeeDetailPage = ({ match }) =>
  <div>
    <h1>Employee {match.params.employeeId} </h1>
  </div>;

export default EmployeeDetailPage;
