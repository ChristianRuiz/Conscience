import React from 'react';

const AccountPicture = ({ pictureUrl }) =>
  <div>
    <div className="picture" style={{ backgroundImage: `url(${pictureUrl})` }} />
  </div>;

AccountPicture.propTypes = {
  pictureUrl: React.PropTypes.string.isRequired
};

export default AccountPicture;
