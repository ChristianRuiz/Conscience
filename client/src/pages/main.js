import React from 'react';
import Routes from '../routes';

import buildConciencePage from './conscience-page';
import Header from '../components/common/Header';
import SignalRClient from '../components/common/SignalRClient';

const enableSignalR = document.location.href.indexOf('.azurewebsites.net') === -1;

const App = () =>
  <div>
    <div>
      <Header />
    </div>

    <div className="mainPageContent">
      <Routes />
    </div>

    { enableSignalR ? <SignalRClient /> : '' }
  </div>;

buildConciencePage(App);
