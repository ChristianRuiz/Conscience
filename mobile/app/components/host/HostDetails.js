import React from 'react';
import {
  View,
  ScrollView,
  Image,
  StyleSheet
} from 'react-native';
import { graphql } from 'react-apollo';
import Spinner from 'react-native-loading-spinner-overlay';

import Background from '../common/Background';
import Divider from '../common/Divider';
import Text from '../common/Text';
import ProfileImage from '../common/ProfileImage';
import ImageUploader from '../common/ImageUploader';

import commonStyles from '../../styles/common';

import query from '../../queries/HostDetailQuery';

const styles = StyleSheet.create({
  image: {
    position: 'absolute',
    top: 40,
    left: 20,
    height: 110,
    width: 110,
    borderRadius: 55
  },
  name: {
    position: 'absolute',
    top: 42,
    left: 165,
    color: '#34FFFC',
    fontWeight: 'bold',
    fontSize: 16,
    width: 140
  },
  serialNumber: {
    position: 'absolute',
    top: 93,
    left: 245,
    width: 55
  },
  battery: {
    position: 'absolute',
    top: 160,
    left: 170,
    width: 55,
    fontWeight: 'bold',
    fontSize: 16
  },
  narrative: {
    position: 'absolute',
    top: 200,
    left: 45,
    width: 260,
    fontSize: 12,
    fontWeight: 'bold',
    lineHeight: 25
  }
});

class HostDetails extends React.Component {
  render() {
    if (this.props.data.loading || !this.props.data.accounts || !this.props.data.accounts.current) {
      return (<View style={commonStyles.container}>
        <Background />
        <Spinner visible />
      </View>);
    }

    const host = this.props.data.accounts.current.host;

/*
<Text>Age: {host.currentCharacter.character.age}</Text>
<Text>Gender: {host.currentCharacter.character.gender}</Text>
*/

    return (<ScrollView>
      <Background />

      <View style={commonStyles.scrollBoxContainer}>

        <ProfileImage style={styles.image} source={host.account.pictureUrl} />

        <Image source={require('../../img/card.png')} style={{ height: 234, width: 299, marginLeft: -10 }} />

        <ImageUploader style={styles.image} />

        {host.currentCharacter ?
          <Text style={styles.name} numberOfLines={1}>
            {host.currentCharacter.character.name}</Text> : <Text />}

        <Text onPress={() => alert('Picture!')} style={styles.serialNumber} numberOfLines={1}>{host.account.userName}</Text>

        {host.account.device ?
          <Text style={styles.battery} numberOfLines={1}>
            {Math.trunc(host.account.device.batteryLevel * 100)}%</Text> : <Text />}

        {host.currentCharacter ?
          <Text style={styles.narrative} numberOfLines={2}>
            {host.currentCharacter.character.narrativeFunction.toUpperCase()} </Text> : <Text />}

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
              <Text>{relation.character.name}: {relation.description}</Text>
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
