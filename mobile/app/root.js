import React from 'react';
import {
  StyleSheet,
  View
} from 'react-native';
import { NativeRouter, Route, Switch } from 'react-router-native';
import {
  ApolloClient,
  ApolloProvider
} from 'react-apollo';
import { createBatchingNetworkInterface } from 'apollo-client';
import { COLOR, ThemeProvider } from 'react-native-material-ui';

import Constants from './constants';

import LoginPage from './pages/LoginPage';
import HostPage from './pages/HostPage';

const client = new ApolloClient({
  networkInterface: createBatchingNetworkInterface({
    uri: `${Constants.SERVER_URL }/api/graphql`,
    batchInterval: 100,
    opts: {
      credentials: 'same-origin'
    }
  })
});

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
        <ApolloProvider client={client}>
          <NativeRouter>
            <View style={styles.container}>
              <Switch>
                <Route exact path="/" component={LoginPage} />
                <Route path="/tabs" component={HostPage} />
              </Switch>
            </View>
          </NativeRouter>
        </ApolloProvider>
      </ThemeProvider>
    );
  }
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#F5FCFF'
  }
});

export default Root;
