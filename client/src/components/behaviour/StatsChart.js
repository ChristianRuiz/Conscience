import React from 'react';
import { Radar } from 'react-chartjs';

class StatsChart extends React.Component {
  componentWillReceiveProps(props) {
    if (this.rendered && props.charData && props.charData.datasets.length) {
      const chart = this.radar.getChart();

      props.charData.datasets[0].data.forEach((value, index) => {
        chart.datasets[0].points[index].value = value;
      });

      chart.update();
    }
  }

  shouldComponentUpdate() {
    return false;
  }

  onRadarClick(event) {
    const points = this.radar.getPointsAtEvent(event);
    if (points.length) {
      const point = points[0];
      this.props.statSelected({ selectedLabel: point.label, selectedValue: point.value });
    }
  }

  render() {
    this.rendered = true;
    return (<Radar
      data={this.props.charData}
      width={this.props.style && this.props.style.width ? this.props.style.width : '600' }
      height={this.props.style && this.props.style.height ? this.props.style.height : '500' }
      onClick={event => this.onRadarClick(event)}
      ref={ref => this.radar = ref}
    />);
  }
}

StatsChart.propTypes = {
  charData: React.PropTypes.object.isRequired,
  statSelected: React.PropTypes.func.isRequired
};

export default StatsChart;
