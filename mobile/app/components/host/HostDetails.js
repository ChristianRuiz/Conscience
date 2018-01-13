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
    top: 115,
    left: 260,
    width: 55,
    fontSize: 14
  },
  narrative: {
    position: 'absolute',
    top: 170,
    left: 45,
    width: 260,
    fontSize: 12,
    fontWeight: 'bold',
    lineHeight: 25
  },
  relationView: {
    marginBottom: 20
  },
  relationTitleView: {
    flexDirection: 'row',
    alignItems: 'center'
  },
  relationPicture: {
    height: 80,
    width: 80,
    borderRadius: 40
  },
  relationName: {
    marginLeft: 15,
    width: 190
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

    const account = this.props.data.accounts.current;
    const host = account.host;

/*
<Text>Age: {host.currentCharacter.character.age}</Text>
<Text>Gender: {host.currentCharacter.character.gender}</Text>
*/

    return (<ScrollView>
      <Background />

      <View style={commonStyles.scrollBoxContainer}>

        <ProfileImage style={styles.image} source={account.pictureUrl} />

        <Image source={require('../../img/card.png')} style={{ height: 274, width: 299, marginLeft: -10 }} />

        <ImageUploader style={styles.image} accountId={account.id} />

        {host.currentCharacter ?
          <Text style={styles.name} numberOfLines={1}>
            {host.currentCharacter.character.name}</Text> : <Text />}

        <Text style={styles.serialNumber} numberOfLines={1}>{account.userName}</Text>

        {account.device ?
          <Text style={styles.battery} numberOfLines={1}>
            {account.device.batteryLevel > 1 ? '100' : Math.trunc(account.device.batteryLevel * 100)}%</Text> : <Text />}

        {host.currentCharacter ?
          <Text style={styles.narrative} numberOfLines={5}>
            {host.currentCharacter.character.narrativeFunction.toUpperCase()} </Text> : <Text />}

        {host.currentCharacter ? (<View>

          <Text style={commonStyles.h3}>STORY</Text>
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
            <View key={relation.id} style={styles.relationView}>
              <View style={styles.relationTitleView}>
                <ProfileImage
                  style={styles.relationPicture}
                  source={relation.character.currentHost ?
                  relation.character.currentHost.host.account.pictureUrl : null}
                />
                <Text style={styles.relationName} numberOfLines={1}>{relation.character.name.toUpperCase()}</Text>
              </View>
              <Text>{relation.description}</Text>
            </View>)}
        </View>) : ''}
      </View>
    </ScrollView>);
  }
}

HostDetails.propTypes = {
  data: React.PropTypes.object.isRequired
};

export default graphql(query)(HostDetails);
