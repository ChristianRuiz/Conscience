import React from 'react';
import { Route, Switch } from 'react-router-dom';

import EmployeesList from './components/employees-list';
import HostsList from './components/hosts-list';
import NotFound from './components/not-found';

const Routes = () =>
  <Switch>
    <Route path="/" exact component={EmployeesList} />
    <Route path="/hosts" component={HostsList} />
    <Route component={NotFound} />
  </Switch>;

export default Routes;
