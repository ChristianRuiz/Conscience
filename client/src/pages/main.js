import React from 'react';

import EmployeePortal from './employeePortal';
import HostApp from './hostApp';
import buildConciencePage from './conscience-page';

const isHostPage = document.location.href.indexOf('#host') > -1;

const App = isHostPage ? HostApp : EmployeePortal;

buildConciencePage(App, !isHostPage);
