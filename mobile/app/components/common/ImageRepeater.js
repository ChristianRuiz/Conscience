import React from 'react';
import { View, Dimensions } from 'react-native';

class ImageRepeater extends React.Component {
  render() {
    let children = this.props.children;

    let height = 0;

    if (this.props.fullscreen) {
      height = Dimensions.get('window').height * 2;
    } else if (this.props.style && this.props.style.height) {
      height = this.props.style.height;
    }

    if (height > 0
    && this.props.children && this.props.children.props.style && this.props.children.props.style.height)
    {
      const imgHeight = this.props.children.props.style.height;

      children = [];

      for(let i=0; i < Math.ceil(height / imgHeight); i++) {
          children.push(this.props.children);
      }
    } else {
      console.warn('Unable to initialize ImageRepeater, make sure you specify the height and the Image height');
    }

    return (<View style={this.props.style}>
      {children}
    </View>);
  }
}

export default ImageRepeater;
