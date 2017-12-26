import React from 'react';
import HostDetail from './CharacterDetail';
import ScrollableContainer from '../common/ScrollableContainer';
import CharacterInfoPanel from '../info-panel/CharacterInfoPanel';

const HostDetailPage = ({ match }) =>
  <div className="mainContainer">
    <ScrollableContainer>
      <HostDetail characterId={match.params.characterId} />
    </ScrollableContainer>
    <CharacterInfoPanel characterId={match.params.characterId} />
  </div>;

HostDetailPage.propTypes = {
  match: React.PropTypes.object.isRequired
};

export default HostDetailPage;
