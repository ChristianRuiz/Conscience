import React from 'react';
import { Link, withRouter } from 'react-router-dom';
import { graphql, gql } from 'react-apollo';
import ScrollableContainer from '../common/ScrollableContainer';

class PlotsList extends React.Component {
  render() {
    if (this.props.data.loading) {
      return (<div>Loading...</div>);
    }

    return (<ScrollableContainer>
      <div>
        <h2>Plots</h2>
        <ul>
          {this.props.data.plots.all.map(plot =>
            <li key={plot.id}>
              <p>
                <Link to={`/plot-detail/${plot.id}`} ><b>{plot.name}</b></Link>
              </p>
            </li>)}
        </ul>
      </div>
    </ScrollableContainer>);
  }
}

PlotsList.propTypes = {
  data: React.PropTypes.object.isRequired
};

const query = gql`query GetPlots {
  plots {
    all {
      id,
      name
    }
  }
}
      `;

export default withRouter(graphql(query)(PlotsList));
