import React from 'react';
import {
  StyleSheet,
  View,
  Image
} from 'react-native';

import ImageRepeater from '../common/ImageRepeater';

import commonStyles from '../../styles/common';

const styles = StyleSheet.create({
  background: {
    position: 'absolute',
    backgroundColor: '#27293E',
    top: 0,
    left: 0,
    bottom: 0,
    right: 0
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
  }
});

class Background extends React.Component {
  render() {
    return (<View style={styles.background} >
      <ImageRepeater style={styles.leftLines} fullscreen >
        <Image source={require('../../img/background/left.png')} style={{height:4, width:11}} />
      </ImageRepeater>
      <View style={styles.rightLines} >
        <Image source={require('../../img/background/right-top.png')} style={{height:174, width:13}} />
        <ImageRepeater style={{height:80}}>
          <Image source={require('../../img/background/right-center.png')} style={{height:1, width:13}} />
        </ImageRepeater>
        <Image source={require('../../img/background/right-bottom.png')} style={{height:46, width:13}} />
      </View>
    </View>);
  }
}

export default Background;
