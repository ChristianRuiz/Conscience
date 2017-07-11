import React from 'react';
import {
  View,
  TouchableHighlight,
  Platform
} from 'react-native';
import ImagePicker from 'react-native-image-picker';
import { withApollo } from 'react-apollo';

import Constants from '../../constants';
import updateCache from '../../services/CacheService';

class ImageUploader extends React.Component {
  constructor(props) {
    super(props);

    this._pickImage = this._pickImage.bind(this);
    this._uploadImage = this._uploadImage.bind(this);
    this._updateImage = this._updateImage.bind(this);
  }

  _pickImage() {
    const options = {
      title: 'Select your picture',
      mediaType: 'photo',
      storageOptions: {
        skipBackup: true,
        path: 'images'
      }
    };

    ImagePicker.showImagePicker(options, (response) => {
      if (!response.didCancel && !response.error && !response.customButton) {
        this._uploadImage(response.fileName, response.uri);
      }
    });
  }

  _uploadImage(fileName, fileURL) {
    const data = new FormData();

    data.append('image', { uri: fileURL, name: fileName, type: `image/${fileName.split('.')[1]}` });

    const config = {
      method: 'POST',
      headers: {
        'Content-Type': 'multipart/form-data'
      },
      body: data
    };

    if (Platform.OS === 'ios') {
      config.headers.Cookie = global.cookieValue;
    }

    return fetch(`${Constants.SERVER_URL}/api/PictureUpload`, config)
      .then(this._updateImage);
  }

  _updateImage(response) {
    response.text().then((url) => {
      updateCache(this.props.client, (data) => {
        data.accounts.current.host.account.pictureUrl = url + '?ts_=' + new Date().toISOString();
      });
    });
  }

  render() {
    return (<TouchableHighlight
      onPress={this._pickImage} style={this.props.style}
      underlayColor="transparent">
      <View style={this.props.style} />
    </TouchableHighlight>);
  }
}

export default withApollo(ImageUploader);
