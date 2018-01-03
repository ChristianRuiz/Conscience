import React from 'react';
import {
    View,
    TouchableHighlight,
    StyleSheet
} from 'react-native';

import Text from '../common/Text';

const styles = StyleSheet.create({
  button: {
    width: 300,
    height: 80,
    borderWidth: 1,
    borderColor: '#276077',
    alignItems: 'center',
    justifyContent: 'center'
  }
});

class Button extends React.Component {
  render() {
    return (<TouchableHighlight {...this.props} style={[styles.button, this.props.style]} >
      <View>
        {this.props.children}
      </View>
    </TouchableHighlight>);
  }
}

export default Button;
