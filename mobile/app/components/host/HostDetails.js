import React from 'react';
import {
  View,
  ScrollView,
  Image
} from 'react-native';
import { graphql } from 'react-apollo';

import Background from '../common/Background';
import Divider from '../common/Divider';
import Text from '../common/Text';
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

<Text style={commonStyles.h3}>{host.account.userName.toUpperCase()}</Text>

{host.currentCharacter ? <Text>{host.currentCharacter.character.name}</Text> : <Text />}

<Text style={commonStyles.p}>
            <Text style={commonStyles.bold}>Narrative function: </Text>
            {host.currentCharacter.character.narrativeFunction}
          </Text>
*/

    return (<ScrollView>
      <Background />

      <View style={commonStyles.scrollBoxContainer}>

        <Image source={require('../../img/sample/card.png')} style={{height:234, width:299, marginLeft: -10}} />

        {host.currentCharacter ? (<View>

          <Text style={commonStyles.h3}>HISTORY</Text>
          <Text>{host.currentCharacter.character.story}</Text>

          <Divider />

          <Text style={commonStyles.h3}>MEMORIES</Text>

          {host.currentCharacter.character.memories.map(memory =>
            <View key={memory.id}>
              <Text>- {memory.description}</Text>
            </View>)}

          <Divider />

          <Text style={commonStyles.h3}>TRIGGERS</Text>

          {host.currentCharacter.character.triggers.map(trigger =>
            <Text key={trigger.id}>- {trigger.description}</Text>)}

          <Divider />

          <Text style={commonStyles.h3}>PLOTLINES</Text>

          {host.currentCharacter.character.plots.map(plot =>
            <View key={plot.id} style={commonStyles.p}>
              <Text style={[commonStyles.bold, commonStyles.center]}>
                --{plot.plot.name}--</Text>
              <Text>{plot.plot.description}</Text>
              <Text style={{ marginTop: 20 }}>{plot.description}</Text>
            </View>)}

          <Divider />

          <Text style={commonStyles.h3}>RELATIONSHIPS</Text>

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
