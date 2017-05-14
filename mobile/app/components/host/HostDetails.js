import React from 'react';
import {
  StyleSheet,
  Text,
  View,
  ScrollView
} from 'react-native';
import { graphql } from 'react-apollo';
import query from '../../queries/HostDetailQuery';

class HostDetails extends React.Component {
  render() {
    if (this.props.data.loading || !this.props.data.accounts.current) {
      return <View style={styles.container}><Text>Loading...</Text></View>;
    }

    const host = this.props.data.accounts.current.host;

    return (<ScrollView>
      <View style={styles.container}>
      <Text style={styles.h3}>{host.account.userName}</Text>

      {host.currentCharacter ? <Text>{host.currentCharacter.character.name}</Text> : <Text />}

      {host.currentCharacter ? (<View>
        <Text>Age: {host.currentCharacter.character.age}</Text>
        <Text>Gender: {host.currentCharacter.character.gender}</Text>


        <Text style={styles.h3}>Story</Text>
        <Text>{host.currentCharacter.character.story}</Text>

        <Text style={styles.p}>
          <Text style={styles.bold}>Narrative function: </Text>
          {host.currentCharacter.character.narrativeFunction}
        </Text>
      </View>) : ''}

      <View>
        <Text style={styles.h3}>Stats summary</Text>

        {host.stats.filter(stat => stat.value !== 10).map(stat =>
          <Text id={stat.name}><Text style={styles.bold}>{stat.name}: </Text> {stat.value}</Text>)}
      </View>
      {host.currentCharacter ? (<View>

        <Text style={styles.h3}>Triggers</Text>

        {host.currentCharacter.character.triggers.map(trigger =>
          <Text id={trigger.id}>{trigger.description}</Text>)}

      </View>) : <View />}
      </View>
    </ScrollView>);
  }
}

HostDetails.propTypes = {
  data: React.PropTypes.object.isRequired
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    padding: 20,
    paddingBottom: 40
  },
  center: {
    justifyContent: 'center',
    alignItems: 'center'
  },
  p: {
    marginTop: 10
  },
  h3: {
    fontSize: 16,
    marginTop: 20,
    marginBottom: 10,
    fontWeight: 'bold'
  },
  bold: {
    fontWeight: 'bold'
  }
});

export default graphql(query)(HostDetails);
