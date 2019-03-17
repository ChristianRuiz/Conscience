import React from 'react';
import { graphql, withApollo } from 'react-apollo';

import ScrollableContainer from '../../common/ScrollableContainer'
import AccountPicture from '../../common/AccountPicture';

import Spinner from '../common/Spinner';
import Background from '../common/Background';
import Divider from '../common/Divider';
import updateCache from '../common/updateCache';

import commonStyles from '../common/styles';

import query from '../../../queries/HostDetailQuery';

const styles = {
    card: {
      backgroundImage: 'url(/content/images/host/card.png)',
        backgroundSize: '300px 284px',
        backgroundRepeat: 'no-repeat',
        width: 284,
        height: 300,
        zIndex: 1,
        paddingRight: 20,
        marginLeft: -10
    },
    image: {
        float: 'left',
        marginTop: 55
    },
    name: {
        paddingTop: 20,
        marginLeft: 130,
      color: '#34FFFC',
      fontWeight: 'bold',
      fontSize: 15,
      width: 169,
      height: 42
    },
    serialNumber: {
        marginTop: -10,
        marginLeft: 220,
      width: 55
    },
    battery: {
        marginTop: -10,
        marginLeft: 232,
      width: 55,
      fontSize: 14
    },
    narrative: {
        marginTop: 35,
        marginLeft: 25,
      width: 260,
      fontSize: 12,
      fontWeight: 'bold'
    },
    relationView: {
      marginBottom: 40
    },
    relationTitleView: {
      display: 'flex',
      flexDirection: 'row',
      alignItems: 'center',
      marginBottom: 30
    },
    relationPicture: {
      height: 80,
      width: 80,
      borderRadius: 40
    },
    relationName: {
      marginLeft: 40
    },
    imageUpload: {
      visibility: 'hidden',
      width: 0,
      height: 0
    }
  };

const uploadImage = (e, client, accountId) => {
  const file = e.target.files[0];

  const data = new FormData();
  data.append('file',file)
  const config = {
    method: 'POST',
    body: data,
    credentials: 'include'
  };

  fetch(`/api/PictureUpload?accountId=${accountId}`, config)
      .then((response) => response.text().then((url) => {
        updateCache(client, (data) => {
          data.accounts.current.pictureUrl = `${url}?_ts=${new Date().getTime()}`;
        });
      }));
}

const Me = ({ data, client }) => {
    if (data.loading || !data.accounts || !data.accounts.current) {
        return (<div style={commonStyles.container}>
          <Background />
          <Spinner visible />
        </div>);
      }
  
    const account = data.accounts.current;
    const host = account.host;

    return (<ScrollableContainer extraMargin={-20}>
    <Background />

    <div style={commonStyles.scrollBoxContainer}>

    <label htmlFor="profile-image-upload">
      <AccountPicture style={styles.image} pictureUrl={account.pictureUrl} />
    </label>
    <input
      id="profile-image-upload"
      type="file"
      style={styles.imageUpload}
      onChange={(e) => uploadImage(e, client, account.id)}
    />

    <div style={styles.card}>
        {host.currentCharacter ?
        <p style={styles.name} numberOfLines={1}>
            {host.currentCharacter.character.name}</p> : <p />}

        <p style={styles.serialNumber} numberOfLines={1}>{account.userName}</p>

        {account.device ?
        <p style={styles.battery} numberOfLines={1}>
            {account.device.batteryLevel > 1 ? '100' : Math.trunc(account.device.batteryLevel * 100)}%</p> : <p />}

        {host.currentCharacter ?
        <p style={styles.narrative} numberOfLines={5}>
            {host.currentCharacter.character.narrativeFunction.toUpperCase()} </p> : <p />}
      </div>

      {host.currentCharacter ? (<div>

        <p style={commonStyles.h3}>STORY</p>
        <p>{host.currentCharacter.character.story.replace(new RegExp('\\n', 'g'), '\n\n')}</p>

        <Divider />

        <p style={commonStyles.h3}>MEMORIES</p>

        {host.currentCharacter.character.memories.map(memory =>
          <div key={memory.id}>
            <p>- {memory.description}</p>
          </div>)}

        <Divider />

        <p style={commonStyles.h3}>TRIGGERS</p>

        {host.currentCharacter.character.triggers.map(trigger =>
          <p key={trigger.id}>- {trigger.description}</p>)}

        <Divider />

        <p style={commonStyles.h3}>PLOTLINES</p>

        {host.currentCharacter.character.plots.map(plot =>
          <div key={plot.id} style={commonStyles.p}>
            <p style={[commonStyles.bold, commonStyles.center]}>
              --{plot.plot.name}--</p>
            <p>{plot.plot.description}</p>
            <p style={{ marginTop: 20 }}>{plot.description}</p>
          </div>)}

        <Divider />

        <p style={commonStyles.h3}>RELATIONSHIPS</p>

        {host.currentCharacter.character.relations.map(relation =>
          <div key={relation.id} style={styles.relationView}>
            <div style={styles.relationTitleView}>
              <AccountPicture
                style={styles.relationPicture}
                pictureUrl={relation.character.currentHost ?
                relation.character.currentHost.host.account.pictureUrl : null}
              />
              <p style={styles.relationName} numberOfLines={1}>{relation.character.name.toUpperCase()}</p>
            </div>
            <p>{relation.description}</p>
          </div>)}
      </div>) : ''}
    </div>
  </ScrollableContainer>);
};

export default withApollo(graphql(query)(Me));
