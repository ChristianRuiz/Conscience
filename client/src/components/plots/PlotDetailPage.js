import React from 'react';

import PlotDetail from './PlotDetail';
import ScrollableContainer from '../common/ScrollableContainer';

const PlotDetailPage = ({ match }) =>
  <div className="mainContainer">
    <ScrollableContainer>
      <PlotDetail plotId={match.params.plotId} />
    </ScrollableContainer>
  </div>;

export default PlotDetailPage;
