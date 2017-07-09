import query from '../queries/HostDetailQuery';

const updateCache = (client, updateFunc) => {
  let data;

  try {
    data = client.readQuery({ query });
  } catch (e) {
    console.log('There is no current account on the cache');
    return;
  }

  try {
    updateFunc(data);
    client.writeQuery({ query, data });
  } catch (e) {
    console.log('Unable to update the cache');
  }
};

export default updateCache;
