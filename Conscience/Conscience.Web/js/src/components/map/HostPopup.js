import React from 'react';

const HostPopup = ({ host }) =>
  <div>
    <p>{host.userName}</p>
  </div>;

HostPopup.propTypes = {
  host: React.PropTypes.object.isRequired
};

export default HostPopup;
