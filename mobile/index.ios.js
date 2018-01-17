import {
  AppRegistry
} from 'react-native';

import Root from './app/root';
import NotificationsService from './app/services/platform/NotificationsServiceIos';

global.iosnotifications = new NotificationsService();
AppRegistry.registerComponent('Conscience', () => Root);
