import React from 'react';
import {
  Text,
  View,
  ScrollView
} from 'react-native';
import { graphql } from 'react-apollo';

import Background from '../common/Background';
import commonStyles from '../../styles/common';

import query from '../../queries/HostDetailQuery';

class HostDetails extends React.Component {
  render() {
    console.log('HostDetail Render');
    if (this.props.data.loading || !this.props.data.accounts || !this.props.data.accounts.current) {
      return (<View style={commonStyles.container}>
        <Background />
        <Text>Loading...</Text>
      </View>);
    }

    console.log('HostDetail Render with data');

    const host = this.props.data.accounts.current.host;

/*
<Text>Age: {host.currentCharacter.character.age}</Text>
<Text>Gender: {host.currentCharacter.character.gender}</Text>
*/

    return (<ScrollView>
      <Background />

      <View style={commonStyles.scrollBoxContainer}>

        <Text style={commonStyles.h3}>{host.account.userName}</Text>

        {host.currentCharacter ? <Text>{host.currentCharacter.character.name}</Text> : <Text />}

        {host.currentCharacter ? (<View>
          <Text style={commonStyles.p}>
            <Text style={commonStyles.bold}>Narrative function: </Text>
            {host.currentCharacter.character.narrativeFunction}
          </Text>

          <Text style={commonStyles.h3}>History</Text>
          <Text>{host.currentCharacter.character.story}</Text>

          <Text style={commonStyles.h3}>Memories</Text>

          {host.currentCharacter.character.memories.map(memory =>
            <View key={memory.id}>
              <Text>{memory.description}</Text>
            </View>)}

          <Text style={commonStyles.h3}>Triggers</Text>

          {host.currentCharacter.character.triggers.map(trigger =>
            <Text key={trigger.id}>{trigger.description}</Text>)}

          <Text style={commonStyles.h3}>Plotlines</Text>

          {host.currentCharacter.character.plots.map(plot =>
            <View key={plot.id} style={commonStyles.p}>
              <Text><Text style={commonStyles.bold}>Plot: </Text> {plot.plot.name}</Text>
              <Text><Text style={commonStyles.bold}b>Plot description: </Text></Text>
              <Text>{plot.plot.description}</Text>
              <Text><Text style={commonStyles.bold}>Character involvement: </Text></Text>
              <Text>{plot.description}</Text>
            </View>)}

          <Text style={commonStyles.h3}>Relationships</Text>

          {host.currentCharacter.character.relations.map(relation =>
            <Text key={relation.id}>
              <Text><b>{relation.character.name}: </b>{relation.description}</Text>
            </Text>)}
        </View>) : ''}
      </View>
    </ScrollView>);
  }
}

HostDetails.propTypes = {
  data: React.PropTypes.object.isRequired
};

export default graphql(query)(HostDetails);
