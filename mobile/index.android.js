import {
  AppRegistry
} from 'react-native';

import Root from './app/root';
import NotificationsService from './app/services/platform/NotificationsServiceAndroid';

AppRegistry.registerHeadlessTask('ConscienceNotificationsService', () => NotificationsService);
AppRegistry.registerComponent('Conscience', () => Root);
