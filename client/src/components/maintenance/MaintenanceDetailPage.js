import React from 'react';
import MaintenanceDetail from './MaintenanceDetail';
import HostsInfoPanel from '../info-panel/HostsInfoPanel';

const MaintenanceDetailPage = ({ match }) =>
  <div className="mainContainer">
    <MaintenanceDetail hostId={match.params.hostId} />
    <HostsInfoPanel hostId={match.params.hostId} />
  </div>;

MaintenanceDetailPage.propTypes = {
  match: React.PropTypes.object.isRequired
};

export default MaintenanceDetailPage;
