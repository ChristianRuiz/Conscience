import React from 'react';

const PlotDetailPage = ({ match }) =>
  <div>
    <h1>Plot {match.params.plotId} </h1>
  </div>;

export default PlotDetailPage;
