import React from 'react';
import {
  Image
} from 'react-native';

class AudioNotification extends React.Component {
  render() {
    return <Image source={require('../../img/background/divider.png')} style={{height:9, width:294, marginLeft: -30, marginTop: 20}} />;
  }
}

AudioNotification.propTypes = {
  notification: React.PropTypes.object.isRequired
};

export default AudioNotification;
