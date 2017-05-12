import React from 'react';
import HostDetail from './HostDetail';

const HostDetailPage = ({ match }) =>
  <div>
    <HostDetail hostId={match.params.hostId} />
  </div>;

HostDetailPage.propTypes = {
  match: React.PropTypes.object.isRequired
};

export default HostDetailPage;
