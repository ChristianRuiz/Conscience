class Constants {
  constructor() {
    this._serverUrls = ['http://192.168.1.100', 'http://192.168.1.110', 'https://consciencelarp.azurewebsites.net'];
    this.serverUrlInitialized = false; // new Date() < new Date(2018, 0, 25);
    this.serverUrlInitializedPendingActions = [];

    this.SERVER_URL = 'https://consciencelarp.azurewebsites.net';
    this.NOTIFICATIONS_INTERVAL_SECONDS = 10;

    this._testServerUrl = this._testServerUrl.bind(this);
    this._initServerUrl = this._initServerUrl.bind(this);
    this.addServerUrlInitializedAction = this.addServerUrlInitializedAction.bind(this);

    this._initServerUrl();
  }

  _testServerUrl(pendingUrls) {
    if (pendingUrls.length === 0) {
      console.warn('Unable to connect to any server');
      setTimeout(this._initServerUrl, 10000);
      return;
    }

    const url = pendingUrls.shift();
    fetch(url).then((result) => {
      if (result.ok) {
        this.SERVER_URL = url;
        this.serverUrlInitialized = true;

        this.serverUrlInitializedPendingActions.forEach(action => action(), this);
      } else {
        this._testServerUrl(pendingUrls);
      }
    }).catch(() => this._testServerUrl(pendingUrls));
  }

  _initServerUrl() {
    const pendingUrls = Array.from(this._serverUrls);
    this._testServerUrl(pendingUrls);
  }

  addServerUrlInitializedAction(action) {
    this.serverUrlInitializedPendingActions.push(action);
  }
}

if (!global.constants) {
  global.constants = new Constants();
}

export default global.constants;
