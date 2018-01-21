import React from 'react';
import { Link, withRouter } from 'react-router-dom';
import { graphql, gql } from 'react-apollo';
import ScrollableContainer from '../common/ScrollableContainer';

import RolesValidation from '../common/RolesValidation';
import Roles from '../../enums/roles';

class PlotsList extends React.Component {
  render() {
    if (this.props.data.loading) {
      return (<div>Loading...</div>);
    }

    return (<ScrollableContainer>
      <div>
        <div className="flex">
          <h2 className="flexStretch">Plots</h2>

          <RolesValidation allowed={[Roles.CompanyPlotEditor, Roles.Admin]}>
            <Link style={{ marginRight: 20 }} to={'/plot-edit/0'} ><h3>New</h3></Link>
          </RolesValidation>
        </div>
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
