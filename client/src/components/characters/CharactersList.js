import React from 'react';
import { Link, withRouter } from 'react-router-dom';
import { graphql, gql } from 'react-apollo';
import ScrollableContainer from '../common/ScrollableContainer';

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

const query = gql`query GetCharacters {
  characters {
    all {
      id,
      name
      currentHost {
        id
        host {
          id
          account {
            id,
            userName
          }
        }
      }
    }
  }
}
      `;

export default withRouter(graphql(query)(CharactersList));
