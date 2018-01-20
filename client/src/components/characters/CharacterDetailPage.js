import React from 'react';
import CharacterDetail from './CharacterDetail';
import ScrollableContainer from '../common/ScrollableContainer';
import CharacterInfoPanel from '../info-panel/CharacterInfoPanel';

const CharacterDetailPage = ({ match }) =>
  <div className="mainContainer">
    <ScrollableContainer>
      <CharacterDetail characterId={match.params.characterId} />
    </ScrollableContainer>
    <CharacterInfoPanel characterId={match.params.characterId} />
  </div>;

CharacterDetailPage.propTypes = {
  match: React.PropTypes.object.isRequired
};

export default CharacterDetailPage;
