import React from 'react';

import styles from '../../styles/components/common/background.css';

const Background = () =>
  <div className="background">
    <div className="background-left">
      <div className="background-left-top">
        <div className="deco-top" />
        <div className="deco-center" />
        <div className="deco-bottom" />
      </div>
      <div className="background-left-bottom">
        <img alt="Aleph. We dream awake." src="/content/images/background/bottom-left-logo.png" />
      </div>
    </div>
    <div className="background-right">
      <div className="deco-top" />
      <div className="deco-center" />
      <div className="deco-bottom" />
    </div>
  </div>;

export default Background;
