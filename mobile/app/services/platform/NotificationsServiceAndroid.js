import BackgroundTimer from 'react-native-background-timer';
import Wakeful from 'react-native-wakeful';
import Constants from '../../constants';

function sleep(ms) {
  return new Promise(resolve => BackgroundTimer.setTimeout(resolve, ms));
}

const AndroidService = async (taskData) => {
  //eslint-disable-next-line
  while(true) {
    if (global.notificationsService) {
      if (!global.wakeLock) {
        const wakeful = new Wakeful();
        wakeful.acquire();
        global.wakeLock = true;
      }
      global.notificationsService.tick();
    }

    //eslint-disable-next-line
    await sleep(Constants.NOTIFICATIONS_INTERVAL_SECONDS * 1000);
  }
};

export default AndroidService;
