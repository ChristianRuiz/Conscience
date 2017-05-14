import React from 'react';
import {
  StyleSheet,
  Text,
  View,
  ScrollView
} from 'react-native';
import { graphql, gql } from 'react-apollo';

class PlotEvents extends React.Component {
  render() {
    if (this.props.data.loading || !this.props.data.accounts.current) {
      return <View style={styles.container}><Text>Loading...</Text></View>;
    }

    const host = this.props.data.accounts.current.host;

    return (<ScrollView>
      <View style={styles.container}>
        {host.currentCharacter ? (<View>
          <Text style={styles.h3}>Plots</Text>

          {host.currentCharacter.character.plots.map(plot =>
            <View id={plot.id} style={styles.p}>
              <Text><Text style={styles.bold}>Plot: </Text> {plot.plot.name}</Text>
              <Text><Text style={styles.bold}b>Plot description: </Text></Text>
              <Text>{plot.plot.description}</Text>
              <Text><Text style={styles.bold}>Character involvement: </Text></Text>
              <Text>{plot.description}</Text>
            </View>)}

          <Text style={styles.h3}>Plot events</Text>

          {host.currentCharacter.character.plotEvents.map(event =>
            <View id={event.id}>
              <Text style={styles.bold}>{event.description}</Text>
              <Text><Text style={styles.bold}>Plot: </Text><Text>{event.plot.name}</Text></Text>
              <Text><Text style={styles.bold}>Location: </Text><Text>{event.location}</Text></Text>
              <Text><Text style={styles.bold}>Time: </Text><Text>{event.hour}:{event.minutes}</Text></Text>
            </View>)}
        </View>) : <View><Text>You have no events assigned</Text></View> }
      </View>
    </ScrollView>);
  }
}

PlotEvents.propTypes = {
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

export default graphql(query)(PlotEvents);

