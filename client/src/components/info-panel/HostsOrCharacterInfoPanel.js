import React from 'react';
import { withRouter } from 'react-router-dom';
import { graphql, gql } from 'react-apollo';

import HostsInfoPanel from './HostsInfoPanel';
import CharacterInfoPanel from './CharacterInfoPanel';

class HostsOrCharacterInfoPanel extends React.Component {
  render() {
    if (this.props.data.loading) {
      return (<div />);
    }

    const host = this.props.data.hosts.byId;

    if (host !== null && host.currentCharacter && host.currentCharacter.character) {
      return <CharacterInfoPanel characterId={host.currentCharacter.character.id} />;
    }

    return <HostsInfoPanel hostId={host.id} />;
  }
}

HostsOrCharacterInfoPanel.propTypes = {
  data: React.PropTypes.object.isRequired
};

const query = gql`query GetHostPanelDetails($hostId:Int!) {
  hosts {
    byId(id: $hostId)
    {
      id
      currentCharacter {
        id
        character {
          id
        }
      }
    }
  }
}
      `;


export default withRouter(graphql(query, { options: { fetchPolicy: 'network-only' } })(HostsOrCharacterInfoPanel));
