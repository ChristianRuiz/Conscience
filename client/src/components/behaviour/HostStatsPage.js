import React from 'react';
import HostStats from './HostStats';
import HostsInfoPanel from '../info-panel/HostsInfoPanel';

const HostStatsPage = ({ match }) =>
  <div className="mainContainer">
    <HostStats hostId={match.params.hostId} />
    <HostsInfoPanel hostId={match.params.hostId} />
  </div>;

HostStatsPage.propTypes = {
  match: React.PropTypes.object.isRequired
};

export default HostStatsPage;
