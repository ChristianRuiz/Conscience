import React from 'react';
import { Link, withRouter } from 'react-router-dom';
import { graphql } from 'react-apollo';
import ScrollableContainer from '../common/ScrollableContainer';

import query from '../../queries/CharactersListQuery';

class CharactersList extends React.Component {
  render() {
    if (this.props.data.loading) {
      return (<div>Loading...</div>);
    }

    const charactersWithHost = this.props.data.characters.all.filter(c => c.currentHost);
    const charactersWithoutHost = this.props.data.characters.all.filter(c => !c.currentHost);

    return (<ScrollableContainer>
      <div>
        <h2>Characters</h2>
        <ul>
          {charactersWithHost.map(character =>
            <li key={character.id}>
              <p>
                <Link to={`/character-detail/${character.id}`} ><b>{character.name}</b></Link>
                {character.currentHost ? ` (${character.currentHost.host.account.userName})` : ''}
              </p>
            </li>)}
        </ul>
        <h2>Unasigned characters</h2>
        <ul>
          {charactersWithoutHost.map(character =>
            <li key={character.id}>
              <p>
                <Link to={`/character-detail/${character.id}`} ><b>{character.name}</b></Link>
                {character.currentHost ? ` (${character.currentHost.host.account.userName})` : ''}
              </p>
            </li>)}
        </ul>
      </div>
    </ScrollableContainer>);
  }
}

CharactersList.propTypes = {
  data: React.PropTypes.object.isRequired
};

export default withRouter(graphql(query)(CharactersList));
