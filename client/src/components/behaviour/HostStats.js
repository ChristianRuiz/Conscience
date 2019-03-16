import React from 'react';
import { withRouter } from 'react-router-dom';
import { graphql, gql, compose } from 'react-apollo';
import Slider from 'rc-slider';
import 'rc-slider/assets/index.css';

import StatsChart from './StatsChart';

import styles from '../../styles/components/behaviour/behaviour.css';

import query from '../../queries/HostStatsQuery';

class HostStats extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      selectedLabel: '',
      selectedValue: 0,
      labels: []
    };
  }

  componentWillReceiveProps(props) {
    if (!props.data.loading && props.data.hosts) {
      const host = props.data.hosts.byId;

      const dataSet = {
        label: `${host.account.userName} stats`,
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

      const newState = { labels, dataSet };

      if (!this.state.selectedLabel) {
        newState.selectedLabel = labels[0];
        newState.selectedValue = dataSet.data[0];
      } else {
        const index = labels.indexOf(this.state.selectedLabel);
        newState.selectedValue = dataSet.data[index];
      }

      this.setState(newState);
    }
  }

  onRadarClick(event) {
    this.setState({ selectedLabel: event.selectedLabel, selectedValue: event.selectedValue });
  }

  onSliderChanged(value) {
    const labelIndex = this.state.labels.indexOf(this.state.selectedLabel);
    const dataSet = Object.assign({}, this.state.dataSet);
    dataSet.data[labelIndex] = value;

    this.setState({ dataSet });

    this.props.mutate({ variables: { hostId: this.props.data.hosts.byId.id, stats: [{ name: this.state.selectedLabel, value }] } });
  }

  render() {
    if (this.props.data.loading || !this.state.selectedLabel) {
      return (<div>Loading...</div>);
    }

    const charData = {
      labels: [...this.state.labels],
      datasets: [this.state.dataSet]
    };

    return (<div className="maintenanceDetailContainer">
      <div className="radarContainer">
        <StatsChart charData={charData} statSelected={event => this.onRadarClick(event)} ref={ref => this.radar = ref} />
      </div>
      <div className="slidersAndTextContainer">
        <div className="slidersContainer">
          <div className="slider">
            <Slider vertical min={0} max={20} value={this.state.selectedValue}
              onChange={value => this.setState({ selectedValue: value })}
              onAfterChange={value => this.onSliderChanged(value)}
            />
          </div>
        </div>
        <div className="text">
          <p>{this.state.selectedValue}</p>
        </div>
        <div className="text">
          <p>{this.state.selectedLabel}</p>
        </div>
      </div>
    </div>);
  }
}

HostStats.propTypes = {
  data: React.PropTypes.object.isRequired,
  mutate: React.PropTypes.func.isRequired
};

const mutation = gql`
mutation ModifyStats($hostId:Int!, $stats:[StatsInput]) {
  hosts {
    modifyStats(hostId: $hostId, stats: $stats)
    {
      id
      stats
      {
        name
        value
      }
    }
  }
}
      `;

export default withRouter(compose(graphql(query, { options: { fetchPolicy: 'network-only' } }), graphql(mutation))(HostStats));
