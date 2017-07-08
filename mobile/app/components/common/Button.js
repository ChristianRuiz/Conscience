import React from 'react';
import { Button } from 'react-native';

import commonStyles from '../../styles/common';

class StyledButton extends React.Component {
  render() {
    return <Button {...this.props} style={[commonStyles.button, this.props.style]} />;
  }
}

export default StyledButton;
