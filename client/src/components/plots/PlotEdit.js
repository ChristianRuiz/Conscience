import React from 'react';
import { withRouter } from 'react-router-dom';
import { Redirect } from 'react-router';
import { graphql, gql, compose } from 'react-apollo';

import TextField from 'material-ui/TextField';
import AutoComplete from 'material-ui/AutoComplete';

import query from '../../queries/PlotDetailQuery';

import AccountPicture from '../common/AccountPicture';

const isEmptyOrSpaces = str => !str || str.match(/^ *$/) !== null;

class PlotEdit extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      editing: false,
      edited: false,
      name: '',
      description: '',
      characters: [],
      events: [],
      autocompleteCharacters: [],
      autocompleteWriters: []
    };
    this.charactersCount = -1;
    this.employeesCount = -1;
  }

  componentWillReceiveProps(props) {
    if (!props.data.loading && !props.autocomplete.loading && !this.state.editing) {
      if (props.data && props.data.plots && props.data.plots.byId) {
        const plot = props.data.plots.byId;

        this.setState({
          editing: true,
          id: plot.id,
          name: plot.name,
          description: plot.description,
          characters: plot.characters,
          events: plot.events,
          writer: plot.writer
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

    if (!state.events.length || !state.events.filter(e => isEmptyOrSpaces(e.description)).length) {
      stateToSet.events = [...state.events, {
        description: '',
        location: '',
        hour: 0,
        minute: 0
      }];
    }

    if (!props.autocomplete.loading && (this.charactersCount < 0 || this.charactersCount !== state.characters.length)) {
      this.charactersCount = state.characters.length;
      stateToSet.autocompleteCharacters = props.autocomplete.characters.all.filter(c =>
        state.characters.filter(r => r.character.id === c.id).length === 0);
    }

    if (!props.autocomplete.loading && (this.employeesCount < 0 || this.employeesCount !== state.autocompleteWriters.length)) {
      stateToSet.autocompleteWriters = props.autocomplete.employees.all.filter(e =>
        e.department === 'Plot' || e.department === 'PlotEditor');
      this.employeesCount = stateToSet.autocompleteWriters.length;
    }

    if (Object.keys(stateToSet).length !== 0) {
      this.setState(stateToSet);
    }
  }

  save() {
    this.props.mutate({
      variables: {
        plot: {
          id: this.state.id,
          name: this.state.name,
          description: this.state.description,
          characters: this.state.characters.filter(c => !isEmptyOrSpaces(c.description)).map(c => ({
            character: { id: c.character.id },
            description: c.description
          })),
          events: this.state.events.filter(e => !isEmptyOrSpaces(e.description)).map(e => ({
            id: e.id,
            description: e.description,
            location: e.location,
            hour: e.hour,
            minute: e.minute
          })),
          writer: {
            id: this.state.writer.id,
            name: this.state.writer.name
          }
        }
      }
    }).then(r => this.setState({ edited: true, plotId: r.data.plots.addOrModifyPlot.id }));
  }

  replaceArrayPosition(array, i, value) {
    const newArray = [...array];
    newArray[i] = value;
    return newArray;
  }

  characterSelected(character, i) {
    if (i !== -1) {
      this.autocompleteCharacters.setState({ searchText: '' });

      this.setState({ characters: [...this.state.characters, {
        character,
        description: ''
      }]
      });
    }
  }

  writerSelected(writer, i) {
    if (i !== -1) {
      this.autocompleteWriters.setState({ searchText: '' });

      this.setState({ writer });
    }
  }

  render() {
    if (this.state.edited) {
      return <Redirect to={`/plot-detail/${this.state.plotId}`} />;
    }

    if (this.props.data.loading) {
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

      <TextField
        multiLine
        rows={1}
        fullWidth
        hintText="Description"
        value={this.state.description}
        onChange={e => this.setState({ description: e.target.value })}
      />

      <div>
        <h2>Characters:</h2>

        <AutoComplete
          floatingLabelText="Add a new character"
          menuStyle={{ backgroundColor: 'black' }}
          filter={AutoComplete.caseInsensitiveFilter}
          dataSource={this.state.autocompleteCharacters}
          dataSourceConfig={{ text: 'name', value: 'id' }}
          ref={ref => this.autocompleteCharacters = ref}
          onNewRequest={(data, i) => this.characterSelected(data, i)}
        />

        <div className="flexColumn marginTop">
          {this.state.characters.map((characterInPlot, i) =>
            <div key={characterInPlot.character.id}>
              <div className="pictureDescriptionBox">
                <AccountPicture pictureUrl={characterInPlot.character.currentHost ? characterInPlot.character.currentHost.host.account.pictureUrl : ''} />
                <div className="titleDescription">
                  <h1>{characterInPlot.character.name}</h1>
                  <TextField
                    floatingLabelText="Characters involment in the plot"
                    name={`relation${characterInPlot.character.id}`}
                    multiLine
                    rows={2}
                    fullWidth
                    value={characterInPlot.description}
                    onChange={e => this.setState({ characters: this.replaceArrayPosition(this.state.characters, i, Object.assign({}, characterInPlot, { description: e.target.value })) })}
                  />
                </div>
              </div>
            </div>)}
        </div>
      </div>

      <div>
        <h2>Events:</h2>

        <div className="flexColumn marginTop">
          {this.state.events.map((event, i) =>
            <div key={`event${i}`}>
              <TextField
                floatingLabelText="Event description"
                name={`eventDescription${i}`}
                value={event.description}
                onChange={e => this.setState({ events: this.replaceArrayPosition(this.state.events, i, Object.assign(event, { description: e.target.value })) })}
              />
              <TextField
                floatingLabelText="Event location"
                name={`eventLocation${i}`}
                value={event.location}
                onChange={e => this.setState({ events: this.replaceArrayPosition(this.state.events, i, Object.assign(event, { location: e.target.value })) })}
              />
              <TextField
                floatingLabelText="Event hour"
                name={`eventHour${i}`}
                type="number"
                value={event.hour}
                onChange={e => this.setState({ events: this.replaceArrayPosition(this.state.events, i, Object.assign(event, { hour: e.target.value })) })}
              />
              <TextField
                floatingLabelText="Event minute"
                name={`eventMinute${i}`}
                type="number"
                value={event.minute}
                onChange={e => this.setState({ events: this.replaceArrayPosition(this.state.events, i, Object.assign(event, { minute: e.target.value })) })}
              />
            </div>)}
        </div>
      </div>

      <AutoComplete
        floatingLabelText="Select the writer"
        menuStyle={{ backgroundColor: 'black' }}
        filter={AutoComplete.caseInsensitiveFilter}
        dataSource={this.state.autocompleteWriters}
        dataSourceConfig={{ text: 'name', value: 'id' }}
        ref={ref => this.autocompleteWriters = ref}
        onNewRequest={(data, i) => this.writerSelected(data, i)}
      />
      {this.state.writer ?
        (<p>Writer: {this.state.writer.name}</p>) : ''}
    </div>);
  }
}

PlotEdit.propTypes = {
  data: React.PropTypes.object.isRequired,
  autocomplete: React.PropTypes.object.isRequired,
  mutate: React.PropTypes.func.isRequired
};


const mutation = gql`
mutation AddOrModifyPlot($plot:PlotInput!) {
  plots {
    addOrModifyPlot(plot: $plot) {
      id
      name
      description
      characters {
        id
        description
        character {
          id
          name
        }
      }
      events {
        id
        description
        location
        hour
        minute
      }
      writer {
        id
        name
      }
    }
  }
}
      `;

const autocomplete = gql`
query PlotEditAutocomplete {
  characters {
    all {
      id
      name
    }
  }
  employees {
    all {
      id
      name
      department
    }
  }
}
      `;

export default withRouter(compose(graphql(query, { options: { fetchPolicy: 'network-only' } }), graphql(mutation), graphql(autocomplete, { name: 'autocomplete', options: { fetchPolicy: 'network-only' } }))(PlotEdit));
