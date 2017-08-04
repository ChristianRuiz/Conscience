import React from 'react';

const MaintenanceDetailPage = ({ match }) =>
  <div>
    <h1>Maintenance {match.params.hostId} </h1>
  </div>;

export default MaintenanceDetailPage;
