import React from 'react';

import { Radar } from 'react-native-pathjs-charts';

class Stats extends React.Component {
  render() {
    const stats = {};
    const data = [stats];

    this.props.stats.filter(stat => stat.value != 10).forEach((stat) => {
      stats[stat.name] = stat.value;
    });

    const options = {
      width: 320,
      height: 320,
      r: 140,
      max: 20,
      fill: '#2980B9',
      stroke: '#2980B9',
      label: {
        fontFamily: 'DejaVuSans',
        fontSize: 14,
        fontWeight: true,
        fill: 'white'
      }
    };

    return <Radar data={data} options={options} />;
  }
}

Stats.propTypes = {
  stats: React.PropTypes.array.isRequired
};

export default Stats;
