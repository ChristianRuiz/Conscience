import React from 'react';
import HostStats from './HostStats';

const HostStatsPage = ({ match }) =>
  <div>
    <HostStats hostId={match.params.hostId} />
  </div>;

HostStatsPage.propTypes = {
  match: React.PropTypes.object.isRequired
};

export default HostStatsPage;
