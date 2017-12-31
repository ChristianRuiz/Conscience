import BackgroundTimer from 'react-native-background-timer';
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

    Sound.setCategory('PlayAndRecord');

    this._soundLoop();
  }

  audioQueue = [];

  _soundLoop() {
    // Playing an empty sound in background to avoid the OS to release our App
    this._playSound('empty.mp3', true).catch(() => {
      BackgroundTimer.setTimeout(() => {
        this._soundLoop();
      }, 1000);
    });
  }

  _playSoundFromFile(filePath, loop) {
    return new Promise((resolve, reject) => {
      // console.log(`loading audio: ${filePath}`);
      if (!loop && this.currentAudio) {
        try {
          this.currentAudio.stop();
          this.currentAudio.release();
        } catch (e) {
        }
      }

      const sound = new Sound(filePath, '', (error) => {
        if (error) {
          console.log('failed to load the sound', error);
          reject('failed to load the sound');
          return;
        }

        if (loop) {
          sound.setNumberOfLoops(-1);
        }

        sound.play((success) => {
          if (success) {
            // console.log('successfully finished playing');
            sound.release();
            resolve();
          } else {
            console.log('playback failed due to audio decoding errors');
            sound.release();
            reject('playback failed');
          }
        });
      });

      if (!loop) {
        this.currentAudio = sound;
      }
    });
  }

  _playSound(fileName, loop = false) {
    return new Promise((resolve, reject) => {
      const self = this;

      const localFileName = fileName.split('/').reverse()[0];

      const filePath = `${RNFS.DocumentDirectoryPath}/${localFileName}`;
      let fileUrl = fileName;
      if (fileUrl.toLowerCase().indexOf('/content') < 0) {
        fileUrl = `/content/audio/${fileName}`;
      }

      RNFS.exists(filePath).then((exists) => {
        if (!exists) {
          RNFS.downloadFile({
            fromUrl: `${Constants.SERVER_URL}${fileUrl}`,
            toFile: filePath
          }).promise.then((r) => {
            console.log('file downloaded');
            console.log(JSON.stringify(r));

            self._playSoundFromFile(filePath, loop).then(resolve).catch(reject);
          });
        } else {
          self._playSoundFromFile(filePath, loop).then(resolve).catch(reject);
        }
      });
    });
  }

  playSound(fileName) {
    return this._playSound(fileName).then(() => {
      if (this.audioQueue.length > 0) {
        const audio = this.audioQueue.shift();
        this.playSound(audio);
      }
    });
  }

  queueSound(fileName) {
    if (this.audioQueue.length === 0) {
      return this.playSound(fileName);
    }

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
