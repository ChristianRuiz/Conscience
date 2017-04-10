import React from 'react';
import Routes from '../routes';

import buildConciencePage from './conscience-page';
import Header from '../components/header';

const App = () =>
  <div>
    <div>
      <Header />
    </div>

    <Routes />
  </div>;

buildConciencePage(App);
