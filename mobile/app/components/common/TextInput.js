import React from 'react';
import {
  TextInput
} from 'react-native';

import commonStyles from '../../styles/common';

class StyledTextInput extends React.Component {
  render() {
    return <TextInput {...this.props} style={[commonStyles.textInput, this.props.style]} />;
  }
}

export default StyledTextInput;
