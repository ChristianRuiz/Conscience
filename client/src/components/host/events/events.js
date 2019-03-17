import React from 'react';
import { graphql } from 'react-apollo';

import ScrollableContainer from '../../common/ScrollableContainer'

import Spinner from '../common/Spinner';
import Background from '../common/Background';

import Divider from '../common/Divider';

import commonStyles from '../common/styles';

import query from '../../../queries/HostDetailQuery';

const styles = {
    plotTitle: {
      marginTop: -20
    },
    detailsLine: {
      width: 260,
      justifyContent: 'space-between',
      flexDirection: 'row'
    },
    details: {
      width: 180
    }
  };

const Events = ({ data }) => {
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

        <p style={[commonStyles.h3, styles.plotTitle]}>PLOT EVENTS</p>

        {host.currentCharacter && host.currentCharacter.character.plotEvents.length > 0 ? (<div>
          {host.currentCharacter.character.plotEvents.map(event =>
            <div key={event.id}>
              <p>- {event.description}</p>
              <p>{event.plot.name}</p>
              <div style={styles.detailsLine}>
                <p>Location: {event.location}</p>
              </div>
              <div style={styles.detailsLine}>
                <p>Time: {event.hour}:{event.minutes ? event.minutes : '00'}</p>
              </div>
              <Divider />
            </div>)}
        </div>) : <div><p>You have no events assigned yet!</p></div> }
      </div>
  </ScrollableContainer>);
};

export default graphql(query)(Events);
