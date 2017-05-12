import RNFS from 'react-native-fs';
import Sound from 'react-native-sound';

import Constants from '../constants';

/*
* Plays an empty 1 second audio contantly to avoid the App to
* close and every second check if there's a real audio to play
*/
class AudioService {

  constructor() {
    this._playSound = this._playSound.bind(this);
    this._playSoundFromFile = this._playSoundFromFile.bind(this);
    this._soundLoop = this._soundLoop.bind(this);
    this.playSound = this.playSound.bind(this);

    Sound.setCategory('Playback');

    //this._soundLoop();
  }

  audioQueue = [];

  _soundLoop() {
    if (this.audioQueue.length > 0) {
      const audio = this.audioQueue.shift();
      this._playSound(audio.fileName).then(() => {
        audio.resolve();
        this._soundLoop();
      }).catch((reason) => {
        audio.reject(reason);
      });
    } else {
      this._playSound('empty.mp3').then(() => {
        this._soundLoop();
      });
    }
  }

  _playSoundFromFile(filePath) {
    return new Promise((resolve, reject) => {
      console.log(`loading audio: ${filePath}`);
      if (this.currentAudio) {
        this.currentAudio.stop();
      }

      this.currentAudio = new Sound(filePath, '', (error) => {
        if (error) {
          console.log('failed to load the sound', error);
          reject('failed to load the sound');
          return;
        }
        // loaded successfully
        console.log(`duration in seconds: ${this.currentAudio.getDuration()}number of channels: ${this.currentAudio.getNumberOfChannels()}`);

        this.currentAudio.play((success) => {
          if (success) {
            console.log('successfully finished playing');
            resolve();
          } else {
            console.log('playback failed due to audio decoding errors');
            this._soundLoop();
          }
        });
      });
    });
  }

  _playSound(fileName) {
    return new Promise((resolve, reject) => {
      const self = this;
      var a = '';
      const localFileName = fileName.split('/').reverse()[0];

      const filePath = `${RNFS.DocumentDirectoryPath}/${localFileName}`;

      RNFS.exists(filePath).then((exists) => {
        if (!exists) {
          RNFS.downloadFile({
            fromUrl: `${Constants.SERVER_URL}/content/audio/${fileName}`,
            toFile: filePath
          }).promise.then((r) => {
            console.log('file downloaded');
            console.log(JSON.stringify(r));

            self._playSoundFromFile(filePath).then(resolve).catch(reject);
          });
        } else {
          self._playSoundFromFile(filePath).then(resolve).catch(reject);
        }
      });
    });
  }

  playSound(fileName) {
    return new Promise((resolve, reject) => {
      this.audioQueue.push({
        fileName,
        resolve,
        reject
      });
    });
  }
}

export default AudioService;
