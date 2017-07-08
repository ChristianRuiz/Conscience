import React from 'react';
import {
  Image
} from 'react-native';

class Divider extends React.Component {
  render() {
    return <Image source={require('../../img/background/divider.png')} style={{height:9, width:294, marginLeft: -30, marginTop: 20}} />;
  }
}

export default Divider;
