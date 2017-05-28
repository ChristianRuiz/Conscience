import Constants from '../constants';

const reportException = (error, isFatal) => {
  try {
    console.log('Reporting error...');

    fetch(`${Constants.SERVER_URL}/api/Errors`, {
      method: 'POST',
      body: JSON.stringify({
        error,
        isFatal
      })
    }).catch((e) => { console.log(`Unable to report error (ajax): ${e}`); }).then(() => {
      console.log(`Error reported '${JSON.stringify(error)}'`);
    });
  } catch (e) {
    console.log(`Unable to report error: ${e}`);
  }
};

export default reportException;
