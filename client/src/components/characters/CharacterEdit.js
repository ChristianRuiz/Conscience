import React from 'react';
import { Link, withRouter } from 'react-router-dom';
import { Redirect } from 'react-router';
import { graphql, gql, compose } from 'react-apollo';

import TextField from 'material-ui/TextField';

import PictureDescriptionBox from '../common/PictureDescriptionBox';

import query from '../../queries/CharacterDetailQuery';

const isEmptyOrSpaces = str => !str || str.match(/^ *$/) !== null;

class CharacterEdit extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      editing: false,
      edited: false,
      name: '',
      narrativeFunction: '',
      story: '',
      triggers: [],
      memories: [],
      plots: [],
      relations: []
    };
  }

  componentWillReceiveProps(props) {
    if (!props.data.loading && !this.state.editing) {
      if (props.data && props.data.characters && props.data.characters.byId) {
        const character = props.data.characters.byId;

        this.setState({
          editing: true,
          id: character.id,
          name: character.name,
          narrativeFunction: character.narrativeFunction,
          story: character.story,
          triggers: character.triggers.map(trigger => ({
            id: trigger.id,
            description: trigger.description
          })),
          memories: character.memories.map(memory => ({
            id: memory.id,
            description: memory.description
          })),
          plots: character.plots.map(p => ({
            plot: p.plot,
            description: p.description
          })),
          relations: character.relations.map(relation => ({
            character: relation.character,
            description: relation.description
          }))
        });
      } else {
        this.setState({
          editing: true
        });
      }
    }
  }

  componentWillUpdate(props, state) {
    const stateToSet = {};

    if (!state.triggers.length || !state.triggers.filter(t => isEmptyOrSpaces(t.description)).length) {
      stateToSet.triggers = [...state.triggers, { description: '' }];
    }

    if (!state.memories.length || !state.memories.filter(m => isEmptyOrSpaces(m.description)).length) {
      stateToSet.memories = [...state.memories, { description: '' }];
    }

    if (Object.keys(stateToSet).length !== 0) {
      this.setState(stateToSet);
    }
  }

  save() {
    this.props.mutate({
      variables: {
        character: {
          id: this.state.id,
          name: this.state.name,
          narrativeFunction: this.state.narrativeFunction,
          story: this.state.story,
          age: 30,
          gender: 'MALE',
          triggers: this.state.triggers.filter(t => !isEmptyOrSpaces(t.description)),
          memories: this.state.memories.filter(t => !isEmptyOrSpaces(t.description)),
          plots: this.state.plots.filter(p => !isEmptyOrSpaces(p.description)).map(p => ({
            plot: { id: p.plot.id },
            description: p.description
          })),
          relations: this.state.relations.filter(r => !isEmptyOrSpaces(r.description)).map(r => ({
            character: { id: r.character.id },
            description: r.description
          }))
        }
      }
    }).then(() => this.setState({ edited: true }));
  }

  render() {
    if (this.state.edited) {
      return <Redirect to="/characters" />;
    }

    if (!this.state.editing) {
      return (<div>Loading...</div>);
    }

    return (<div>
      <div className="flex">
        <div className="flexStretch">
          <TextField
            hintText="Name"
            value={this.state.name}
            onChange={e => this.setState({ name: e.target.value })}
          />
        </div>
        <button style={{ marginRight: 20 }} className="linkButton" onClick={() => this.save()}><h3>Save</h3></button>
      </div>
      <div>
        <h2>Character Story:</h2>
        <p>{this.state.story}</p>
      </div>

      <h2>Triggers:</h2>

      <ul>
        {this.state.triggers.map((trigger, i) =>
          <li key={i}>{trigger.description}</li>)}
      </ul>

      <h2>Memories:</h2>

      <ul>
        {this.state.memories.map((memory, i) =>
          <li key={i}>{memory.description}</li>)}
      </ul>

      <h2>Plots:</h2>

      <ul>
        {this.state.plots.map(p =>
          <li key={p.plot.id}>
            <p><Link to={`/plot-detail/${p.plot.id}`} >{p.plot.name}</Link>: {p.plot.description}</p>
            <p><b>Character involvement: </b></p>
            <p>{p.description}</p>
          </li>)}
      </ul>

      <div>
        <h2>Relations:</h2>

        <div className="flexColumn marginTop">
          {this.state.relations.map(relation =>
            <PictureDescriptionBox
              key={relation.character.id}
              pictureUrl={relation.character.currentHost ? relation.character.currentHost.host.account.pictureUrl : ''}
              title={relation.character.name}
              link={`/character-detail/${relation.character.id}`}
              description={relation.description}
            />)}
        </div>
      </div>
    </div>);
  }
}

CharacterEdit.propTypes = {
  data: React.PropTypes.object.isRequired,
  mutate: React.PropTypes.func.isRequired
};

const mutation = gql`
mutation AddOrModifyCharacter($character:CharacterInput!) {
  characters {
    addOrModifyCharacter(character: $character) {
      id
      name
      age
      story
      narrativeFunction
      gender
      memories {
        id
        description
      }
      triggers {
        id
        description
      }
      plots {
        id
        description
        plot {
          id
        }
      }
      relations {
        id
        description
        character {
          id
        }
      }
    }
  }
}
      `;

export default withRouter(compose(graphql(query), graphql(mutation))(CharacterEdit));
