import React from 'react';
import { withRouter } from 'react-router-dom';
import { graphql, gql } from 'react-apollo';
import { Radar } from 'react-chartjs';

class HostStats extends React.Component {
  render() {
    if (this.props.data.loading) {
      return (<div>Loading...</div>);
    }

    const host = this.props.data.hosts.byId;

    const dataSet = {
      label: host.account.userName + ' stats',
      fillColor: 'rgba(151,187,205,0.2)',
      strokeColor: 'rgba(151,187,205,1)',
      pointColor: 'rgba(151,187,205,1)',
      pointStrokeColor: '#fff',
      pointHighlightFill: '#fff',
      pointHighlightStroke: 'rgba(151,187,205,1)',
      data: []
    };
    const chartData = {
      labels: [],
      datasets: [dataSet]
    };

    host.stats.forEach((stat) => {
      chartData.labels.push(stat.name);
      dataSet.data.push(stat.value);
    }, this);

    return (<div className="mainContent itemsCentered">
      <Radar data={chartData} width="600" height="500" />
    </div>);
  }
}

HostStats.propTypes = {
  data: React.PropTypes.object.isRequired
};

const query = gql`query GetHostDetails($hostId:Int!) {
  hosts {
    byId(id:$hostId)
    {
      id,
      account {
        id,
        userName
      },
      currentCharacter {
        id,
        character {
          id,
          name
        }
      },
      stats {
        id,
        name,
        value
      }
    }
  }
}
      `;

export default withRouter(graphql(query)(HostStats));
