import React from 'react';
import HostStats from './HostStats';
import HostsOrCharacterInfoPanel from '../info-panel/HostsOrCharacterInfoPanel';

const HostStatsPage = ({ match }) =>
  <div className="mainContainer">
    <HostStats hostId={match.params.hostId} />
    <HostsOrCharacterInfoPanel hostId={match.params.hostId} />
  </div>;

HostStatsPage.propTypes = {
  match: React.PropTypes.object.isRequired
};

export default HostStatsPage;
