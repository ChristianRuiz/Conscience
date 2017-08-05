import React from 'react';

const AccountPicture = ({ pictureUrl }) =>
  <div>
    <div className="picture" style={{ backgroundImage: `url('${pictureUrl}'), url('/Content/images/card/defaultProfile.png')` }} />
  </div>;

AccountPicture.propTypes = {
  pictureUrl: React.PropTypes.string.isRequired
};

export default AccountPicture;
