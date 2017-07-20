import React from 'react';
import {
  StyleSheet,
  View,
  Platform
} from 'react-native';
import { NativeRouter, Route, Switch } from 'react-router-native';
import {
  ApolloClient,
  ApolloProvider
} from 'react-apollo';
import { createBatchingNetworkInterface, createNetworkInterface } from 'apollo-client';
import { COLOR, ThemeProvider } from 'react-native-material-ui';

import Constants from './constants';

import LoginPage from './pages/LoginPage';
import HostPage from './pages/HostPage';

import reportException from './services/ReportException';

import commonStyles from './styles/common';

const defaultHandler = ErrorUtils.getGlobalHandler && ErrorUtils.getGlobalHandler() || ErrorUtils._globalHandler;

async function wrapGlobalHandler(error, isFatal) {
  reportException(error, isFatal);

  if (__DEV__) {
    // after you're finished, call the defaultHandler so that react-native also gets the error
    defaultHandler(error, isFatal);
  }
}

ErrorUtils.setGlobalHandler(wrapGlobalHandler);


const createNetwork = Platform.OS === 'ios' ? createNetworkInterface : createBatchingNetworkInterface;

const networkInterface = createNetwork({
  uri: `${Constants.SERVER_URL}/api/graphql`,
  batchInterval: 10,
  opts: {
    credentials: 'same-origin'
  }
});

Constants.addServerUrlInitializedAction(() => {
  networkInterface._uri = `${Constants.SERVER_URL}/api/graphql`;
});

// Hack: iOS is not sending the cookie credentials, so we force them
if (Platform.OS === 'ios') {
  networkInterface.useAfter([{
    applyAfterware({ response }, next) {
      if (response.headers) {
        const cookie = response.headers.get('Set-Cookie');
        if (cookie) {
          global.cookieValue = cookie.split(';')[0];
        }
      }
      next();
    }
  }]);

  networkInterface.use([{
    applyMiddleware(req, next) {
      if (global.cookieValue) {
        if (!req.options.headers) {
          req.options.headers = {};
        }
        req.options.headers.Cookie = global.cookieValue;
      }
      next();
    }
  }]);
}

const client = new ApolloClient({
  networkInterface
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
            <View style={commonStyles.mainContainer}>
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

export default Root;
