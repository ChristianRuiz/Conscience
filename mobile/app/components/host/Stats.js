import React from 'react';
import {
  View,
  ScrollView,
  StyleSheet
} from 'react-native';
import { graphql } from 'react-apollo';
import Spinner from 'react-native-loading-spinner-overlay';

import Background from '../common/Background';
import Text from '../common/Text';
import Divider from '../common/Divider';
import StatsChart from './StatsChart';
import commonStyles from '../../styles/common';

import query from '../../queries/HostDetailQuery';

const styles = StyleSheet.create({
  statsTitle: {
    marginTop: -20
  },
  statSummaryLine: {
    width: 130,
    justifyContent: 'space-between',
    flexDirection: 'row'
  },
  statName: {
    width: 105
  }
});

class Stats extends React.Component {
  render() {
    if (this.props.data.loading || !this.props.data.accounts || !this.props.data.accounts.current) {
      return (<View style={commonStyles.container}>
        <Background />
        <Spinner visible />
      </View>);
    }

    const host = this.props.data.accounts.current.host;

    return (<ScrollView>
      <Background />

      <View style={commonStyles.scrollBoxContainer}>

        <View>
          <Text style={[commonStyles.h3, styles.statsTitle]}>STATS SUMMARY</Text>

          {host.stats.filter(stat => stat.value !== 10).map(stat =>
            <View style={styles.statSummaryLine} key={stat.name}>
              <Text style={styles.statName} numberOfLines={1}>-{stat.name}: </Text>
              <Text>{stat.value}</Text>
            </View>)}

          <Divider />

          <StatsChart stats={host.stats} />
        </View>
      </View>
    </ScrollView>);
  }
}

Stats.propTypes = {
  data: React.PropTypes.object.isRequired
};

export default graphql(query)(Stats);
