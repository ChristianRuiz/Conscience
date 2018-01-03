import React from 'react';

import HostDetails from '../host/HostDetails';
import PlotEvents from '../host/PlotEvents';
import Stats from '../host/Stats';
import Notifications from '../notifications/Notifications';
import Buttons from '../buttons/Buttons';

const routes = [
  { key: '1', title: 'Me', icon: 'ios-contact' },
  { key: '2', title: 'Stats', icon: 'ios-body' },
  { key: '3', title: 'Events', icon: 'ios-calendar' },
  { key: '4', title: 'Nots', icon: 'ios-notifications' },
  { key: '5', title: 'Status', icon: 'ios-warning' }
];

const renderScene = ({ route }) => {
  switch (route.key) {
    case '1':
      return (
        <HostDetails />
      );
    case '2':
      return (
        <Stats />
      );
    case '3':
      return (
        <PlotEvents />
      );
    case '4':
      return (
        <Notifications />
      );
    case '5':
      return (
        <Buttons />
      );
    default:
      return null;
  }
};

export default { routes, renderScene, notificationsTab: '4' };
