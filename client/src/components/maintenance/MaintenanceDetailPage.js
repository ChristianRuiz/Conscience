import React from 'react';
import MaintenanceDetail from './MaintenanceDetail';
import HostsOrCharacterInfoPanel from '../info-panel/HostsOrCharacterInfoPanel';

const MaintenanceDetailPage = ({ match }) =>
  <div className="mainContainer">
    <MaintenanceDetail hostId={match.params.hostId} />
    <HostsOrCharacterInfoPanel hostId={match.params.hostId} />
  </div>;

MaintenanceDetailPage.propTypes = {
  match: React.PropTypes.object.isRequired
};

export default MaintenanceDetailPage;
