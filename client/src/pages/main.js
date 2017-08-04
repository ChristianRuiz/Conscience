import React from 'react';
import Routes from '../routes';

import buildConciencePage from './conscience-page';
import Header from '../components/common/Header';
import SignalRClient from '../components/common/SignalRClient';

const App = () =>
  <div>
    <div>
      <Header />
    </div>

    <div className="mainPageContent">
      <Routes />
    </div>

    <SignalRClient />
  </div>;

buildConciencePage(App);
