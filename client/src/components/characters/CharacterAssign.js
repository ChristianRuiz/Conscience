import React from 'react';
import { withRouter } from 'react-router-dom';
import { Redirect } from 'react-router';
import { graphql, gql, compose } from 'react-apollo';

import AutoComplete from 'material-ui/AutoComplete';

import PictureDescriptionBox from '../common/PictureDescriptionBox';

import formatDate from '../../utls/DateFormatter';

import query from '../../queries/CharacterDetailQuery';

class CharacterAssign extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      editing: false,
      edited: false,
      character: null,
      hostToAssign: null,
      autocompleteHosts: []
    };
  }

  componentWillReceiveProps(props) {
    if (!props.data.loading && !props.autocomplete.loading && !this.state.editing) {
      if (props.data && props.data.characters && props.data.characters.byId) {
        const character = props.data.characters.byId;

        this.setState({
          editing: true,
          character
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

    if (!props.autocomplete.loading && !state.autocompleteHosts.length && state.character) {
      stateToSet.autocompleteHosts = props.autocomplete.hosts.all.filter(h => !h.currentCharacter || h.currentCharacter.id !== state.character.id).map(h => ({
        id: h.id,
        display: h.account.userName + (h.currentCharacter ? ` - ${h.currentCharacter.character.name}` : ''),
        host: h
      }));
    }

    if (Object.keys(stateToSet).length !== 0) {
      this.setState(stateToSet);
    }
  }

  save() {
    this.props.mutate({
      variables: {
        hostId: this.state.hostToAssign.id,
        characterId: this.state.character.id
      }
    }).then(() => this.setState({ edited: true, characterId: this.state.character.id }));
  }

  hostSelected(h, i) {
    if (i !== -1) {
      this.autocompleteHosts.setState({ searchText: '' });

      this.setState({
        hostToAssign: h.host
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

    const character = this.props.data.characters.byId;

    return (<div>
      <div className="flex">
        <h1 className="flexStretch">{character.name}</h1>
        <button style={{ marginRight: 20 }} className="linkButton" onClick={() => this.save()}><h3>Assign</h3></button>
      </div>

      { character.currentHost ? (<div>
        <h2>Assigned Host:</h2>

        <PictureDescriptionBox
          pictureUrl={character.currentHost.host.account.pictureUrl}
          title={character.currentHost.host.account.userName}
          link={`/behaviour-detail/${character.currentHost.host.id}`}
          description={`Current character: ${character.name}. Assigned on: ${formatDate(character.currentHost.assignedOn)}`}
        />
      </div>) : '' }

      <div>
        <h2>New Host:</h2>

        <AutoComplete
          floatingLabelText="Select the host to assign"
          menuStyle={{ backgroundColor: 'black' }}
          filter={AutoComplete.caseInsensitiveFilter}
          dataSource={this.state.autocompleteHosts}
          dataSourceConfig={{ text: 'display', value: 'id' }}
          ref={ref => this.autocompleteHosts = ref}
          onNewRequest={(data, i) => this.hostSelected(data, i)}
        />

        { this.state.hostToAssign ? (<div>
          <PictureDescriptionBox
            pictureUrl={this.state.hostToAssign.account.pictureUrl}
            title={this.state.hostToAssign.account.userName}
            link={`/behaviour-detail/${this.state.hostToAssign.id}`}
            description={this.state.hostToAssign.currentCharacter ? `Current character: ${this.state.hostToAssign.currentCharacter.character.name}. Assigned on: ${formatDate(this.state.hostToAssign.currentCharacter.assignedOn)}` : ''}
          />
        </div>) : '' }
      </div>
    </div>);
  }
}

CharacterAssign.propTypes = {
  data: React.PropTypes.object.isRequired,
  autocomplete: React.PropTypes.object.isRequired,
  mutate: React.PropTypes.func.isRequired
};

const mutation = gql`
mutation AssignCharacter($hostId:Int!, $characterId:Int!) {
  hosts {
    assignCharacter(hostId: $hostId, characterId: $characterId) {
      id
      currentCharacter {
        id
        assignedOn
        unassignedOn
        character {
          id
        }
      }
      characters {
        id
        assignedOn
        unassignedOn
        character {
          id
        }
      }
    }
  }
}
      `;

const autocomplete = gql`
query CharacterAssignAutocomplete {
  hosts {
    all {
      id
      account {
        id
        userName
        pictureUrl
      }
      currentCharacter {
        id
        assignedOn
        character{
          id
          name
        }
      }
    }
  }
}
      `;

export default withRouter(compose(graphql(query, { options: { fetchPolicy: 'network-only' } }), graphql(mutation), graphql(autocomplete, { name: 'autocomplete', options: { fetchPolicy: 'network-only' } }))(CharacterAssign));
