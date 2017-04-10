import React from 'react';
import ReactDOM from 'react-dom';
import { HashRouter as Router } from 'react-router-dom';
import {
  ApolloClient,
  ApolloProvider
} from 'react-apollo';
import { createBatchingNetworkInterface } from 'apollo-client';
import MuiThemeProvider from 'material-ui/styles/MuiThemeProvider';
import injectTapEventPlugin from 'react-tap-event-plugin';

const client = new ApolloClient({
  networkInterface: createBatchingNetworkInterface({
    uri: '/api/graphql',
    batchInterval: 10,
    opts: {
      credentials: 'same-origin'
    }
  })
});

export default function buildConciencePage(rootComponent) {
        // Needed for onTouchTap
  injectTapEventPlugin();

  ReactDOM.render(
    <MuiThemeProvider>
      <ApolloProvider client={client}>
        <Router>
          { React.createElement(rootComponent) }
        </Router>
      </ApolloProvider>
    </MuiThemeProvider>
    , document.getElementById('main'));
}
