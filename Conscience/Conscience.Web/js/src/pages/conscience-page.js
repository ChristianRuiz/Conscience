import React from 'react';
import ReactDOM from 'react-dom';
import Relay from 'react-relay'
import MuiThemeProvider from 'material-ui/styles/MuiThemeProvider';

export default class ConsciencePage
{
    constructor(rootComponent, mainRoute)
    {
        Relay.injectNetworkLayer(new Relay.DefaultNetworkLayer('/api/graphql', { credentials: 'same-origin' }));

        ReactDOM.render(<MuiThemeProvider>
                            <Relay.RootContainer Component={rootComponent}
                                route={new mainRoute()} />
                        </MuiThemeProvider>, document.getElementById('main'));
    }
}