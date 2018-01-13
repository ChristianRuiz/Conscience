import React from 'react';
import HostDetail from './HostDetail';
import ScrollableContainer from '../common/ScrollableContainer';
import HostsOrCharacterInfoPanel from '../info-panel/HostsOrCharacterInfoPanel';

const HostDetailPage = ({ match }) =>
  <div className="mainContainer">
    <ScrollableContainer>
      <HostDetail hostId={match.params.hostId} />
    </ScrollableContainer>
    <HostsOrCharacterInfoPanel hostId={match.params.hostId} />
  </div>;

HostDetailPage.propTypes = {
  match: React.PropTypes.object.isRequired
};

export default HostDetailPage;
