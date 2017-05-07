import React from 'react';
import { Route, Switch } from 'react-router-dom';

import MapPage from './components/map/MapPage';
import EmployeesPage from './components/employees/EmployeesPage';
import HostsPage from './components/hosts/HostPage';
import HostClientTestPage from './components/host-client-test/HostClientTestPage';
import BulkImportPage from './components/bulk-import/BulkImportPage';
import NotFound from './components/common/NotFound';

const Routes = () =>
  <Switch>
    <Route path="/" exact component={MapPage} />
    <Route path="/hosts" component={HostsPage} />
    <Route path="/employees" exact component={EmployeesPage} />
    <Route path="/client-test" component={HostClientTestPage} />
    <Route path="/bulk-import" component={BulkImportPage} />
    <Route component={NotFound} />
  </Switch>;

export default Routes;
