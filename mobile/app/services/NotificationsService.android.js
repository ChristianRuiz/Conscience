import BackgroundTimer from 'react-native-background-timer';

function sleep(ms) {
  return new Promise(resolve => BackgroundTimer.setTimeout(resolve, ms));
}

const AndroidService = async (taskData) => {
  //eslint-disable-next-line
  while(true) {
    if (global.notificationsService) {
      global.notificationsService.tick();
    }

    //eslint-disable-next-line
    await sleep(15 * 1000);
  }
};

export default AndroidService;
