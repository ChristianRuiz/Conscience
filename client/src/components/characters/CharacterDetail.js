import React from 'react';
import { Link, withRouter } from 'react-router-dom';
import { graphql } from 'react-apollo';

import RolesValidation from '../common/RolesValidation';
import Roles from '../../enums/roles';

import PictureDescriptionBox from '../common/PictureDescriptionBox';

import query from '../../queries/CharacterDetailQuery';

class CharacterDetail extends React.Component {
  render() {
    if (this.props.data.loading) {
      return (<div>Loading...</div>);
    }

    const character = this.props.data.characters.byId;

    return (<div>
      <div className="flex">
        <h1 className="flexStretch">{character.name}</h1>

        <RolesValidation allowed={[Roles.CompanyPlotEditor, Roles.Admin]}>
          <Link style={{ marginRight: 20 }} to={`/character-edit/${character.id}`} ><h3>Edit</h3></Link>
        </RolesValidation>
      </div>
      <div>
        <h2>Character Story:</h2>
        <p>{character.story}</p>
      </div>

      <h2>Triggers:</h2>

      <ul>
        {character.triggers.map(trigger =>
          <li key={trigger.id}>{trigger.description}</li>)}
      </ul>

      <h2>Memories:</h2>

      <ul>
        {character.memories.map(memory =>
          <li key={memory.id}>{memory.description}</li>)}
      </ul>

      <h2>Plots:</h2>

      <ul>
        {character.plots.map(p =>
          <li key={p.plot.id}>
            <p><Link to={`/plot-detail/${p.plot.id}`} >{p.plot.name}</Link>: {p.plot.description}</p>
            <p><b>Character involvement: </b></p>
            <p>{p.description}</p>
          </li>)}
      </ul>

      {character.plotEvents.length > 0 ? (<div>
        <h2>Plot events:</h2>

        <ul>
          {character.plotEvents.map(event =>
            <li key={event.id}>
              <p><b>{event.description}</b></p>
              <p><b>Plot: </b>{event.plot.name}</p>
              <p><b>Location: </b>{event.location}</p>
              <p><b>Time: </b>{event.hour}:{event.minutes}</p>
            </li>)}
        </ul></div>) : '' }

      {character.relations.length > 0 ? (<div>
        <h2>Relations:</h2>

        <div className="flexColumn marginTop">
          {character.relations.map(relation =>
            <PictureDescriptionBox
              key={relation.id}
              pictureUrl={relation.character.currentHost ? relation.character.currentHost.host.account.pictureUrl : ''}
              title={relation.character.name}
              link={`/character-detail/${relation.character.id}`}
              description={relation.description}
            />)}
        </div>
      </div>) : '' }
    </div>);
  }
}

CharacterDetail.propTypes = {
  data: React.PropTypes.object.isRequired
};

export default withRouter(graphql(query)(CharacterDetail));
