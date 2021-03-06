import React from 'react';
import { Route, Switch } from 'react-router-dom';

import MapPage from './components/map/MapPage';
import PlotsPage from './components/plots/PlotsPage';
import PlotDetailPage from './components/plots/PlotDetailPage';
import CharactersPage from './components/characters/CharactersPage';
import HostDetailPage from './components/characters/HostDetailPage';
import BehaviourPage from './components/behaviour/BehaviourPage';
import HostStatsPage from './components/behaviour/HostStatsPage';
import MaintenancePage from './components/maintenance/MaintenancePage';
import MaintenanceDetailPage from './components/maintenance/MaintenanceDetailPage';
import SecurityPage from './components/security/SecurityPage';
import EmployeeDetailPage from './components/security/EmployeeDetailPage';
import HostClientTestPage from './components/host-client-test/HostClientTestPage';
import BulkImportPage from './components/bulk-import/BulkImportPage';
import NotFound from './components/common/NotFound';

const Routes = () =>
  <Switch>
    <Route path="/" exact component={MapPage} />
    <Route path="/plots" component={PlotsPage} />
    <Route path="/plot-detail/:plotId" component={PlotDetailPage} />
    <Route path="/characters" component={CharactersPage} />
    <Route path="/character-detail/:hostId" component={HostDetailPage} />
    <Route path="/behaviour" component={BehaviourPage} />
    <Route path="/behaviour-detail/:hostId" component={HostStatsPage} />
    <Route path="/maintenance" component={MaintenancePage} />
    <Route path="/maintenance-detail/:hostId" component={MaintenanceDetailPage} />
    <Route path="/security" exact component={SecurityPage} />
    <Route path="/security-detail/:employeeId" component={EmployeeDetailPage} />
    <Route path="/client-test" component={HostClientTestPage} />
    <Route path="/bulk-import" component={BulkImportPage} />
    <Route component={NotFound} />
  </Switch>;

export default Routes;
