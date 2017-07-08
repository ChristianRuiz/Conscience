import React from 'react';
import {
  Text
} from 'react-native';

import commonStyles from '../../styles/common';

class StyledText extends React.Component {
  render() {
    return <Text {...this.props} style={[commonStyles.text, this.props.style]} />;
  }
}

export default StyledText;
