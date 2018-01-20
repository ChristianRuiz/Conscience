import React from 'react';
import { withRouter } from 'react-router-dom';

import PlotEdit from './PlotEdit';
import ScrollableContainer from '../common/ScrollableContainer';

const PlotEditPage = ({ match }) =>
  <div className="mainContainer">
    <ScrollableContainer>
      <PlotEdit plotId={match.params.plotId} />
    </ScrollableContainer>
  </div>;

export default withRouter(PlotEditPage);
