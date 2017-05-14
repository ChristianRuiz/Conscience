import React from 'react';

const HostPopup = ({ account }) =>
  <div>
    <p><b>{account.userName}</b></p>
    <p>{account.host.currentCharacter ? account.host.currentCharacter.character.name : ''}</p>
  </div>;

HostPopup.propTypes = {
  account: React.PropTypes.object.isRequired
};

export default HostPopup;
