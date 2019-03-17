import React from 'react';

const AccountPicture = ({ pictureUrl, style }) =>
  <div style={style}>
    <div className="picture" style={{ backgroundImage: `url('${pictureUrl}'), url('/Content/images/card/defaultProfile.png')` }} />
  </div>;

AccountPicture.defaultProps = {
  pictureUrl: ''
};

AccountPicture.propTypes = {
  pictureUrl: React.PropTypes.string
};

export default AccountPicture;
