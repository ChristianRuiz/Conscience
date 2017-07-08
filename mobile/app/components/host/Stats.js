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

class Stats extends React.Component {
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
        
        <View>
          <Text style={commonStyles.h3}>Stats summary</Text>

          {host.stats.filter(stat => stat.value !== 10).map(stat =>
            <Text key={stat.name}><Text style={commonStyles.bold}>{stat.name}
              : </Text> {stat.value}</Text>)}
        </View>
      </View>
    </ScrollView>);
  }
}

Stats.propTypes = {
  data: React.PropTypes.object.isRequired
};

export default graphql(query)(Stats);
