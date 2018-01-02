import React from 'react';
import { Button, Platform } from 'react-native';

import commonStyles from '../../styles/common';

class StyledButton extends React.Component {
  render() {
    return (Platform.OS === 'ios' ?
      <Button {...this.props} style={[commonStyles.button, this.props.style]} color="white" />
      :
      <Button {...this.props} style={[commonStyles.button, this.props.style]} />);
  }
}

export default StyledButton;
