import React from 'react';
import Drawer from 'material-ui/Drawer';

const InfoPanel = ({ host }) => {
  if (host !== null) {
    return (<Drawer open={host !== null} openSecondary>
      <p>Name: {host.account.userName}</p>
      <p>Online: {host.account.device.online}</p>
    </Drawer>);
  }

  return <div />;
};

InfoPanel.propTypes = {
  host: React.PropTypes.object
};

InfoPanel.defaultProps = {
  host: null
};

export default InfoPanel;
