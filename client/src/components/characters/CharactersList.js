import React from 'react';
import { Link, withRouter } from 'react-router-dom';
import { graphql } from 'react-apollo';
import ScrollableContainer from '../common/ScrollableContainer';

import RolesValidation from '../common/RolesValidation';
import Roles from '../../enums/roles';

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
        <div className="flex">
          <h2 className="flexStretch">Characters</h2>

          <RolesValidation allowed={[Roles.CompanyPlotEditor, Roles.Admin]}>
            <Link style={{ marginRight: 20 }} to={'/character-edit/0'} ><h3>New</h3></Link>
          </RolesValidation>
        </div>
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
