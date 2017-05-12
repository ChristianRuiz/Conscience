import DeviceBattery from 'react-native-device-battery';

class SignalRService {
    constructor(navigator) {
        this.addListener = this.addListener.bind(this);
        this._onTimer = this._onTimer.bind(this);
        
        const onBatteryStateChanged = (state) => {
            const initialized = this.batteryLevel;

            this.batteryLevel = state.level;
            this.charging = state.charging;

            if (!initialized) {
                this._onTimer();
            }
        };

        DeviceBattery.addListener(onBatteryStateChanged);

        this.locations = [];
        this.locationsInitilized = false;

        this.watchID = navigator.geolocation.watchPosition((location) => {
            this.locations.push(location);

            if (!this.locationsInitilized) {
                this.locationsInitilized = true;
                this._onTimer();
            }
        });

        setInterval(this._onTimer, 1000 * 15);
    }

    listener = null;

    addListener(listener) {
        this.listener = listener;
        this._onTimer();
    }

    _onTimer() {
        if (this.listener) {
            var update = {};

            if (this.batteryLevel) {
                Object.assign(update, {
                    batteryLevel: this.batteryLevel,
                    charging: this.charging
                })
            }

            if (this.locations.length > 0) {
                //TODO: Consolidate locations
                Object.assign(update, {
                    locations: this.locations
                });

                this.locations = [];
            }

            this.listener(update);
        }
    }
}

export default SignalRService;