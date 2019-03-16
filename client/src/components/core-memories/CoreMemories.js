import React from 'react';
import { withRouter } from 'react-router-dom';
import { graphql, gql, compose } from 'react-apollo';
import Checkbox from 'material-ui/Checkbox';

import ScrollableContainer from '../common/ScrollableContainer';

import styles from '../../styles/components/core-memories/coreMemories.css';

const checkStyle = { fill: 'white' };

class CoreMemories extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      hosts: [],
      selectAll: false
    };
  }

  componentWillReceiveProps(props) {
    if (!props.data.loading && props.data.hosts) {
      this.setState({ hosts: props.data.hosts.all.map(host => ({
        selected: false,
        id: host.id,
        name: host.account.userName,
        character: host.currentCharacter ? host.currentCharacter.character.name : '',
        cm1: host.coreMemory1 && host.coreMemory1.locked,
        cm2: host.coreMemory2 && host.coreMemory2.locked,
        cm3: host.coreMemory3 && host.coreMemory3.locked,
        cm1Empty: !host.coreMemory1,
        cm2Empty: !host.coreMemory2,
        cm3Empty: !host.coreMemory3
      }))
      });
    }
  }

  selectAll() {
    const selectAll = !this.state.selectAll;
    const hosts = this.state.hosts.map(host => Object.assign(host, { selected: selectAll }));
    this.setState({ hosts, selectAll });
  }

  updateCMSelected(i) {
    const hosts = this.state.hosts;
    hosts[i].selected = !hosts[i].selected;
    this.setState({ hosts });
  }

  unlockMemory(coreMemoryId) {
    this.state.hosts.filter(host => host.selected).forEach(host =>
      this.props.mutate({ variables: {
        hostId: host.id,
        coreMemoryId
      }
      }));
  }

  render() {
    if (this.props.data.loading) {
      return (<div>Loading...</div>);
    }

    return (<ScrollableContainer>
      <div className="coreMemories">
        <div className="flex">
          <h2 className="flexStretch">Core Memories</h2>
        </div>
        <div>
          <button className="linkButton" onClick={() => this.unlockMemory(1)}><h3>Unlock 1</h3></button>
          <button className="linkButton" onClick={() => this.unlockMemory(2)}><h3>Unlock 2</h3></button>
          <button className="linkButton" onClick={() => this.unlockMemory(3)}><h3>Unlock 3</h3></button>
        </div>
        <Checkbox
          iconStyle={checkStyle}
          label="All"
          value={this.state.selectAll}
          onCheck={() => this.selectAll()}
          className="checkbox"
        />
        <ul>
          {this.state.hosts.map((host, i) =>
            <li key={host.id}>
              <div className="flex">
                <Checkbox
                  iconStyle={checkStyle}
                  label=""
                  checked={host.selected}
                  onCheck={() => this.updateCMSelected(i)}
                  className="checkbox"
                />
                <span className="cmUser">{host.name}</span>
                <span className="cmCharacter">{host.character}</span>
                <span className="coreMemory">1: {host.cm1Empty ? 'EMPTY' : (host.cm1 ? 'LOCKED' : 'UNLOCKED')}</span>
                <span className="coreMemory">2: {host.cm2Empty ? 'EMPTY' : (host.cm2 ? 'LOCKED' : 'UNLOCKED')}</span>
                <span className="coreMemory">3: {host.cm3Empty ? 'EMPTY' : (host.cm3 ? 'LOCKED' : 'UNLOCKED')}</span>
              </div>
            </li>)}
        </ul>
      </div>
    </ScrollableContainer>);
  }
}

CoreMemories.propTypes = {
  data: React.PropTypes.object.isRequired,
  mutate: React.PropTypes.func.isRequired
};

const query = gql`query GetCoreMemories {
  hosts {
    all {
      id
      account {
        id
        userName
      }
      currentCharacter {
        id
        character {
          id
          name
        }
      }
      coreMemory1 {
        id
        locked
      }
      coreMemory2 {
        id
        locked
      }
      coreMemory3 {
        id
        locked
      }
    }
  }
}
      `;

const mutation = gql`
mutation UnlockCoreMemories($hostId:Int!, $coreMemoryId:Int!) {
  hosts {
    unlockCoreMemory(hostId: $hostId, coreMemoryId: $coreMemoryId)
    {
      id
      coreMemory1 {
        id
        locked
      }
      coreMemory2 {
        id
        locked
      }
      coreMemory3 {
        id
        locked
      }
    }
  }
}
      `;

export default withRouter(compose(graphql(query, { options: { fetchPolicy: 'network-only' } }), graphql(mutation))(CoreMemories));
