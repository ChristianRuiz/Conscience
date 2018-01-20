import React from 'react';
import CharacterEdit from './CharacterEdit';
import ScrollableContainer from '../common/ScrollableContainer';
import CharacterInfoPanel from '../info-panel/CharacterInfoPanel';

const CharacterEditPage = ({ match }) =>
  <div className="mainContainer">
    <ScrollableContainer>
      <CharacterEdit characterId={match.params.characterId} />
    </ScrollableContainer>
    {match.params.characterId ? <CharacterInfoPanel characterId={match.params.characterId} /> : ''}
  </div>;

CharacterEditPage.propTypes = {
  match: React.PropTypes.object.isRequired
};

export default CharacterEditPage;
