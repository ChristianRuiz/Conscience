import { React, PropTypes } from 'react';

import {
  StyleSheet,
  Text,
  View
} from 'react-native';

class Background extends React.Component {
  render() {
    return (<View style={styles.container}>
      {this.props.children}
    </View>);
  }
}

Background.propTypes = {
  children: PropTypes.node.isRequired
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center'
  }
});

export default Background;
