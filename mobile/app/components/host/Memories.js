import React from 'react';
import {
  StyleSheet,
  Text,
  View,
  ScrollView
} from 'react-native';
import { graphql, gql } from 'react-apollo';

class Memories extends React.Component {
  render() {
    if (this.props.data.loading || !this.props.data.accounts.current) {
      return <View style={styles.container}><Text>Loading...</Text></View>;
    }

    const host = this.props.data.accounts.current.host;

    return (<ScrollView>
      <View style={styles.container}>
        {host.currentCharacter && host.currentCharacter.character.memories.length > 0 ? (<View>
            
          <Text style={styles.h3}>Memories</Text>

          {host.currentCharacter.character.memories.map(memory =>
            <View id={memory.id}>
              <Text>{memory.description}</Text>
            </View>)}
        </View>) : <View><Text>You have no memories</Text></View> }

        {host.currentCharacter && host.currentCharacter.character.relations.length > 0 ? (<View>
          <Text style={styles.h3}>Relations</Text>

          {host.currentCharacter.character.relations.map(relation =>
            <Text id={relation.id}>
              <Text><b>{relation.character.name}: </b>{relation.description}</Text>
            </Text>)}
        </View>) : <View /> }
      </View>
    </ScrollView>);
  }
}

Memories.propTypes = {
  data: React.PropTypes.object.isRequired
};

const query = gql`{
  accounts {
    current {
      host {
        id
        account {
          userName
          device {
            lastConnection
            online
            batteryLevel
          }
        }
        currentCharacter {
          assignedOn
          character {
            name
            age
            gender
            story
            narrativeFunction
            triggers {
              id
              description
            }
            plots {
              id
              plot {
                name
                description
              }
              description
            }
            memories {
              id
              description
            }
            relations {
              id
              description
              character {
                name
              }
            }
            plotEvents {
              id
              plot {
                name
              }
              description
              location
              hour
              minute
              characters {
                id
                name
              }
            }
          }
        }
        stats {
          name
          value
        }
      }
    }
  }
}
`;

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

export default graphql(query)(Memories);