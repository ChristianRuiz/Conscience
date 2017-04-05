import React from 'react';
import ReactDOM from 'react-dom';
import {
  ApolloClient,
  createNetworkInterface,
  ApolloProvider,
} from 'react-apollo';
import MuiThemeProvider from 'material-ui/styles/MuiThemeProvider';
import injectTapEventPlugin from 'react-tap-event-plugin';

const client = new ApolloClient({
      networkInterface: createNetworkInterface({
        uri: '/api/graphql',
        opts: {
            credentials: 'same-origin',
        }
      }),
    });

export default class ConsciencePage
{
    constructor(rootComponent)
    {
        // Needed for onTouchTap
        injectTapEventPlugin();

        ReactDOM.render(<MuiThemeProvider>
                            <ApolloProvider client={client}>
                                { React.createElement(rootComponent) }
                            </ApolloProvider>
                        </MuiThemeProvider>, document.getElementById('main'));
    }
}