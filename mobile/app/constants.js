class Constants {
  constructor() {
    this.serverUrlChangedActions = [];

    this.SERVER_URL = 'https://consciencelarp.azurewebsites.net';
    this.NOTIFICATIONS_INTERVAL_SECONDS = 10;
  }

  changeServerUrl(url) {
    this.SERVER_URL = url;
    
    this.serverUrlChangedActions.forEach(action => action(), this);
  }

  serverUrlChanged(action) {
    this.serverUrlChangedActions.push(action);
  }
}

if (!global.constants) {
  global.constants = new Constants();
}

export default global.constants;
