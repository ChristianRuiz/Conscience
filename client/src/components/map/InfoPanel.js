import React from 'react';

const InfoPanel = ({ account }) => {
  if (account !== null) {
    return (<div className="infoPanel">
      <div className="picture" style={{ backgroundImage: `url(${account.pictureUrl})` }} />
      <div className="card">
        <h1 className="charName">{account.host.currentCharacter ? account.host.currentCharacter.character.name : ''}</h1>
        <p className="serialNumber">#{account.userName}</p>
        <p className="online">{account.device.online ? 'On Line' : 'Off Line'}</p>
      </div>
    </div>);
  }

  return <div />;
};

InfoPanel.propTypes = {
  account: React.PropTypes.object
};

InfoPanel.defaultProps = {
  account: null
};

export default InfoPanel;
