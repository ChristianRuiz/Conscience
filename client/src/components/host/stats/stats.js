import React from 'react';
import { graphql } from 'react-apollo';

import ScrollableContainer from '../../common/ScrollableContainer'
import StatsChart from '../../behaviour/StatsChart';

import Spinner from '../common/Spinner';
import Background from '../common/Background';
import Divider from '../common/Divider';

import commonStyles from '../common/styles';

import query from '../../../queries/HostDetailQuery';

const styles = {
    statsTitle: {
      marginTop: -20
    },
    statSummaryLine: {
      width: 130,
      justifyContent: 'space-between',
      flexDirection: 'row'
    },
    statName: {
      width: 300
    }
  };

const getCharData = (account, host) => {
    const dataSet = {
        label: `${account.userName} stats`,
        fillColor: 'rgba(151,187,205,0.2)',
        strokeColor: 'rgba(151,187,205,1)',
        pointColor: 'rgba(151,187,205,1)',
        pointStrokeColor: '#fff',
        pointHighlightFill: '#fff',
        pointHighlightStroke: 'rgba(151,187,205,1)',
        data: []
      };

      const labels = [];

      host.stats.forEach((stat) => {
        labels.push(stat.name);
        dataSet.data.push(stat.value);
      }, this);

      const charData = {
        labels: [...labels],
        datasets: [dataSet]
      };

      return charData;
}

const Stats = ({ data }) => {
    if (data.loading || !data.accounts || !data.accounts.current) {
        return (<div style={commonStyles.container}>
          <Background />
          <Spinner visible />
        </div>);
      }
  
      const account = data.accounts.current;
      const host = account.host;

      return (<ScrollableContainer extraMargin={-20}>
        <Background />
  
        <div style={commonStyles.scrollBoxContainer}>
  
          <div>
            <p style={[commonStyles.h3, styles.statsTitle]}>STATS SUMMARY</p>
  
            {host.stats.filter(stat => stat.value !== 10).map(stat =>
              <div style={styles.statSummaryLine} key={stat.name}>
                <p style={styles.statName} numberOfLines={1}>-{stat.name}: {stat.value}</p>
              </div>)}
  
            <Divider />
  
            <StatsChart charData={getCharData(account, host)} style={{ width: 300, height: 300 }} />
          </div>
        </div>
      </ScrollableContainer>);
};

export default graphql(query)(Stats);
