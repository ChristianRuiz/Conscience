import React from 'react';

import ImageRepeater from './ImageRepeater';

const styles = {
  background: {
    position: 'absolute',
    backgroundColor: '#27293E',
    top: 0,
    left: 0,
    bottom: 0,
    right: 0,
    zIndex: -1
  },
  leftLines: {
    position: 'absolute',
    width: 11,
    top: 0,
    left: 0,
    bottom: 0
  },
  rightLines: {
    position: 'absolute',
    flex: 1,
    width: 13,
    top: 0,
    right: 0,
    bottom: 0
  },
  leftLinesView: {
    position: 'absolute',
    width: 5.4,
    top: -1,
    left: 5.5,
    bottom: 0,
    border: '1px solid #276077'
  }
};

class Background extends React.Component {
  render() {
    return (<div style={styles.background} >
      <div style={styles.leftLinesView} />
      <div style={styles.rightLines} >
        <img src="/content/images/host/background/right-top.png" style={{height:174, width:13}} />
        <ImageRepeater src="/content/images/host/background/right-center.png" srcSize={{height: 1, width:13}} style={{height:80}} />
        <img src="/content/images/host/background/right-bottom.png" style={{height:46, width:13}} />
      </div>
    </div>);
  }
}

export default Background;
