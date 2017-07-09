import React from 'react';
import {
  View,
  ScrollView,
  StyleSheet
} from 'react-native';
import { graphql } from 'react-apollo';

import Background from '../common/Background';
import Text from '../common/Text';
import Divider from '../common/Divider';
import commonStyles from '../../styles/common';

import query from '../../queries/HostDetailQuery';

const styles = StyleSheet.create({
  plotTitle: {
    marginTop: -20
  },
  detailsLine: {
    width: 260,
    justifyContent: 'space-between',
    flexDirection: 'row'
  },
  details: {
    width: 180
  }
});

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

        <Text style={[commonStyles.h3, styles.plotTitle]}>PLOT EVENTS</Text>

        {host.currentCharacter && host.currentCharacter.character.plotEvents.length > 0 ? (<View>
          {host.currentCharacter.character.plotEvents.map(event =>
            <View key={event.id}>
              <Text>- {event.description}</Text>
              <Text>{event.plot.name}</Text>
              <View style={styles.detailsLine}>
                <Text>Location: </Text>
                <Text style={styles.details} numberOfLines={1}>{event.location}</Text>
              </View>
              <View style={styles.detailsLine}>
                <Text>Time: </Text>
                <Text style={styles.details} numberOfLines={1}>{event.hour}:{event.minutes ? event.minutes : '00'}</Text>
              </View>
              <Divider />
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

