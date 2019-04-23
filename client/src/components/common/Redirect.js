import React from 'react';

const Redirect = ({ href }) => {
  document.location.href = href;

  return <div />;
};

Redirect.propTypes = {
  href: React.PropTypes.string.isRequired
};

export default Redirect;
