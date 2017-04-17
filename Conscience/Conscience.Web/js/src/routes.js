import React from 'react';
import { Route, Switch } from 'react-router-dom';

import MapPage from './components/map/MapPage';
import EmployeesPage from './components/employees/EmployeesPage';
import HostsPage from './components/hosts/HostPage';
import NotFound from './components/common/NotFound';

const Routes = () =>
  <Switch>
    <Route path="/" exact component={EmployeesPage} />
    <Route path="/hosts" component={HostsPage} />
    <Route path="/map" component={MapPage} />
    <Route component={NotFound} />
  </Switch>;

export default Routes;
