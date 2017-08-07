import React from 'react';
import { Link } from 'react-router-dom';

import AccountPicture from './AccountPicture';

import styles from '../../styles/components/common/pictureDescriptionBox.css';

const PictureDescriptionBox = ({ pictureUrl, title, link, description }) =>
  <div className="pictureDescriptionBox">
    <Link to={link}><AccountPicture pictureUrl={pictureUrl} /></Link>
    <div className="titleDescription">
      <h1><Link to={link}>{title}</Link></h1>
      <p>{description}</p>
    </div>
  </div>;

PictureDescriptionBox.propTypes = {
  pictureUrl: React.PropTypes.string.isRequired,
  title: React.PropTypes.string.isRequired,
  link: React.PropTypes.string.isRequired,
  description: React.PropTypes.string.isRequired
};

export default PictureDescriptionBox;
