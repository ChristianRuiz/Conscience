import React from 'react';
import {
  StyleSheet,
  Text,
  View
} from 'react-native';
import { COLOR, ThemeProvider } from 'react-native-material-ui';
import Debugger from './components/Debugger';

const uiTheme = {
  palette: {
    primaryColor: COLOR.green500
  },
  toolbar: {
    container: {
      height: 50
    }
  }
};

class Root extends React.Component {
  render() {
    return (
      <ThemeProvider>
        <View style={styles.container}>
          <Debugger />
        </View>
      </ThemeProvider>
    );
  }
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: '#F5FCFF'
  }
});

export default Root;
