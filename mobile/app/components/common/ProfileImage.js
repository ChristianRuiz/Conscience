import React from 'react';
import {
  Image,
  StyleSheet
} from 'react-native';

import Constants from '../../constants';

const styles = StyleSheet.create({
  image: {
    height: 110,
    width: 110,
    borderRadius: 55
  }
});

class ProfileImage extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      errorLoadingImage: false
    };
  }

  _getDefaultProfileImage() {
    return require('../../img/defaultProfile.png');
  }

  render() {
    let source = this._getDefaultProfileImage();

    if (!this.state.errorLoadingImage && this.props.source) {
      source = { uri: Constants.SERVER_URL + this.props.source };
    }

    return (<Image
      source={source} style={[styles.image, this.props.style]}
      onError={() => this.setState({ errorLoadingImage: true })}
    />);
  }
}

ProfileImage.propTypes = {
  source: React.PropTypes.any
};

ProfileImage.defaultProps = {
  source: ''
};

export default ProfileImage;
