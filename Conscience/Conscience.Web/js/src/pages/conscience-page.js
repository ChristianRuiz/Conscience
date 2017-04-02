import React from 'react';
import ReactDOM from 'react-dom';
import Relay from 'react-relay'
import MuiThemeProvider from 'material-ui/styles/MuiThemeProvider';
import injectTapEventPlugin from 'react-tap-event-plugin';

export default class ConsciencePage
{
    constructor(rootComponent, mainRoute)
    {
        // Needed for onTouchTap
        injectTapEventPlugin();

        Relay.injectNetworkLayer(new Relay.DefaultNetworkLayer('/api/graphql', { credentials: 'same-origin' }));

        if (mainRoute) {
            ReactDOM.render(<MuiThemeProvider>
                                <Relay.RootContainer Component={rootComponent}
                                    route={new mainRoute()} />
                            </MuiThemeProvider>, document.getElementById('main'));
        } else
        {
            ReactDOM.render(<MuiThemeProvider>
                                { React.createElement(rootComponent) }
                            </MuiThemeProvider>, document.getElementById('main'));
        }
    }
}