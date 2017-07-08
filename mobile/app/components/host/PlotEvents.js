import React from 'react';
import {
  Text,
  View,
  ScrollView
} from 'react-native';
import { graphql } from 'react-apollo';

import Background from '../background/Background';
import commonStyles from '../../styles/common';

import query from '../../queries/HostDetailQuery';

class PlotEvents extends React.Component {
  render() {
    if (this.props.data.loading || !this.props.data.accounts || !this.props.data.accounts.current) {
      return (<View style={commonStyles.container}>
        <Background />
        <Text>Loading...</Text>
      </View>);
    }

    const host = this.props.data.accounts.current.host;

    return (<ScrollView>
      <Background />

      <View style={commonStyles.scrollBoxContainer}>

        {host.currentCharacter && host.currentCharacter.character.plotEvents.length > 0 ? (<View>
          <Text style={commonStyles.h3}>Plot events</Text>

          {host.currentCharacter.character.plotEvents.map(event =>
            <View key={event.id}>
              <Text style={commonStyles.bold}>{event.description}</Text>
              <Text><Text style={commonStyles.bold}>Plot: </Text>
                <Text>{event.plot.name}</Text>
              </Text>
              <Text><Text style={commonStyles.bold}>Location: </Text>
                <Text>{event.location}</Text>
              </Text>
              <Text><Text style={commonStyles.bold}>Time: </Text>
                <Text>{event.hour}:{event.minutes}</Text>
              </Text>
            </View>)}
        </View>) : <View><Text>You have no events assigned yet!</Text></View> }
      </View>
    </ScrollView>);
  }
}

PlotEvents.propTypes = {
  data: React.PropTypes.object.isRequired
};

export default graphql(query)(PlotEvents);

