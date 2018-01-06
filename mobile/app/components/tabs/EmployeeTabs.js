import React from 'react';

import EmployeeDetails from '../employee/EmployeeDetails';
import Notifications from '../notifications/Notifications';
import Buttons from '../buttons/Buttons';

const routes = [
  { key: '1', title: 'Me', icon: 'ios-contact' },
  { key: '2', title: 'Nots', icon: 'ios-notifications' },
  { key: '3', title: 'Panic', icon: 'ios-warning' }
];

const renderScene = ({ route }) => {
  switch (route.key) {
    case '1':
      return (
        <EmployeeDetails />
      );
    case '2':
      return (
        <Notifications />
      );
    case '3':
      return (
        <Buttons />
      );
    default:
      return null;
  }
};

export default { routes, renderScene, notificationsTab: '2' };
