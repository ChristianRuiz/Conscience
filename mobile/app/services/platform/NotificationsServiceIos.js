import BackgroundTimer from 'react-native-background-timer';
import Location from 'react-native-location';
import Constants from '../../constants';

class IOSService {
  constructor() {
    this._onTimer = this._onTimer.bind(this);

    Location.requestAlwaysAuthorization();

    if (this.intervalId) {
      BackgroundTimer.clearInterval(this.intervalId);
    }

    this.intervalId = BackgroundTimer.setInterval(() => {
      this._onTimer();
    }, 1000 * Constants.NOTIFICATIONS_INTERVAL_SECONDS);

    this._onTimer();
  }

  _onTimer() {
    console.log('iOS timer tick');

    if (global.notificationsService) {
      global.notificationsService.tick();
    }
  }
}

export default IOSService;
