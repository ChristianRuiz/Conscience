import React from 'react';
import { withRouter } from 'react-router-dom';
import { Redirect } from 'react-router';
import { graphql, gql, compose } from 'react-apollo';

import TextField from 'material-ui/TextField';
import AutoComplete from 'material-ui/AutoComplete';

import AccountPicture from '../common/AccountPicture';

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
      relations: [],
      autocompletePlots: [],
      autocompleteCharacters: []
    };
    this.plotsCount = -1;
    this.relationsCount = -1;
  }

  componentWillReceiveProps(props) {
    if (!props.data.loading && !props.autocomplete.loading && !this.state.editing) {
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

    if (!props.autocomplete.loading && (this.plotsCount < 0 || this.plotsCount !== state.plots.length)) {
      this.plotsCount = state.plots.length;
      stateToSet.autocompletePlots = props.autocomplete.plots.all.filter(p =>
        state.plots.filter(sp => sp.plot.id === p.id).length === 0);
    }

    if (!props.autocomplete.loading && (this.relationsCount < 0 || this.relationsCount !== state.relations.length)) {
      this.relationsCount = state.relations.length;
      stateToSet.autocompleteCharacters = props.autocomplete.characters.all.filter(c =>
        state.relations.filter(r => r.character.id === c.id).length === 0);
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
    }).then(r => this.setState({ edited: true, characterId: r.data.characters.addOrModifyCharacter.id }));
  }

  replaceArrayPosition(array, i, value) {
    const newArray = [...array];
    newArray[i] = value;
    return newArray;
  }

  plotSelected(plot, i) {
    if (i !== -1) {
      this.autocompletePlots.setState({ searchText: '' });

      this.setState({ plots: [...this.state.plots, {
        plot,
        description: ''
      }]
      });
    }
  }

  characterSelected(character, i) {
    if (i !== -1) {
      this.autocompleteCharacters.setState({ searchText: '' });

      this.setState({ relations: [...this.state.relations, {
        character,
        description: ''
      }]
      });
    }
  }

  render() {
    if (this.state.edited) {
      return <Redirect to={`/character-detail/${this.state.characterId}`} />;
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
        <TextField
          multiLine
          rows={4}
          fullWidth
          hintText="Story"
          value={this.state.story}
          onChange={e => this.setState({ story: e.target.value })}
        />
      </div>

      <h2>Triggers:</h2>

      <ul>
        {this.state.triggers.map((trigger, i) =>
          <li key={i}>
            <TextField
              name={`trigger${i}`}
              fullWidth
              value={trigger.description}
              onChange={e => this.setState({ triggers: this.replaceArrayPosition(this.state.triggers, i, { id: trigger.id, description: e.target.value }) })}
            />
          </li>)}
      </ul>

      <h2>Memories:</h2>

      <ul>
        {this.state.memories.map((memory, i) =>
          <li key={i}>
            <TextField
              name={`memory${i}`}
              fullWidth
              value={memory.description}
              onChange={e => this.setState({ memories: this.replaceArrayPosition(this.state.memories, i, { id: memory.id, description: e.target.value }) })}
            />
          </li>)}
      </ul>

      <h2>Plots:</h2>

      <AutoComplete
        floatingLabelText="Add a new plot"
        menuStyle={{ backgroundColor: 'black' }}
        filter={AutoComplete.caseInsensitiveFilter}
        dataSource={this.state.autocompletePlots}
        dataSourceConfig={{ text: 'name', value: 'id' }}
        ref={ref => this.autocompletePlots = ref}
        onNewRequest={(data, i) => this.plotSelected(data, i)}
      />

      <ul>
        {this.state.plots.map((p, i) =>
          <li key={p.plot.id}>
            <p><b>{p.plot.name}:</b> {p.plot.description}</p>
            <TextField
              floatingLabelText="Character involvement"
              name={`plot${p.plot.id}`}
              multiLine
              rows={2}
              fullWidth
              value={p.description}
              onChange={e => this.setState({ plots: this.replaceArrayPosition(this.state.plots, i, Object.assign(p, { description: e.target.value })) })}
            />
          </li>)}
      </ul>

      <div>
        <h2>Relations:</h2>

        <AutoComplete
          floatingLabelText="Add a new relation"
          menuStyle={{ backgroundColor: 'black' }}
          filter={AutoComplete.caseInsensitiveFilter}
          dataSource={this.state.autocompleteCharacters}
          dataSourceConfig={{ text: 'name', value: 'id' }}
          ref={ref => this.autocompleteCharacters = ref}
          onNewRequest={(data, i) => this.characterSelected(data, i)}
        />

        <div className="flexColumn marginTop">
          {this.state.relations.map((relation, i) =>
            <div className="pictureDescriptionBox" key={relation.character.id}>
              <AccountPicture pictureUrl={relation.character.currentHost ? relation.character.currentHost.host.account.pictureUrl : ''} />
              <div className="titleDescription">
                <h1>{relation.character.name}</h1>
                <TextField
                  floatingLabelText="Characters relation"
                  name={`relation${relation.character.id}`}
                  multiLine
                  rows={2}
                  fullWidth
                  value={relation.description}
                  onChange={e => this.setState({ relations: this.replaceArrayPosition(this.state.relations, i, Object.assign(relation, { description: e.target.value })) })}
                />
              </div>
            </div>)}
        </div>
      </div>
    </div>);
  }
}

CharacterEdit.propTypes = {
  data: React.PropTypes.object.isRequired,
  autocomplete: React.PropTypes.object.isRequired,
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

const autocomplete = gql`
query CharacterEditAutocomplete {
  plots {
    all {
      id
      name
    }
  }
  characters {
    all {
      id
      name
    }
  }
}
      `;

export default withRouter(compose(graphql(query), graphql(mutation), graphql(autocomplete, { name: 'autocomplete' }))(CharacterEdit));
