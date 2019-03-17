import React from 'react';
import { graphql, withApollo } from 'react-apollo';

import ScrollableContainer from '../../common/ScrollableContainer'

import Spinner from '../common/Spinner';
import Background from '../common/Background';

import Divider from '../common/Divider';

import commonStyles from '../common/styles';

import query from '../../../queries/HostDetailQuery';

import RaisedButton from 'material-ui/RaisedButton';

const styles = {
    notificationsLine: {
      justifyContent: 'space-between',
      flexDirection: 'row',
      marginTop: 20
    },
    modal: {
      padding: 20,
      backgroundColor: '#194353'
    },
    transcriptText: {
      marginBottom: 30
    },
    coreMemotyTitle: {
      marginBottom: 10,
      color: '#F8BB3E'
    },
    errorText: {
      color: 'red'
    }
  };

class Notifications extends React.Component {
    state = {
        refreshing: false,
        errorRefreshing: false
    };

    refresh() {
        this.setState({ refreshing: true, errorRefreshing: false });

        this.props.client.query({
            fetchPolicy: 'network-only',
            fetchResults: true,
            query
        }).catch((error) => {
            this.setState({ refreshing: false, errorRefreshing: true });
        }).then(() => {
            this.setState({ refreshing: false });
        });
    }

    render() {
        const { data } = this.props;

        if (this.state.refreshing || data.loading || !data.accounts || !data.accounts.current) {
            return (<div style={commonStyles.container}>
            <Background />
            <Spinner visible />
            </div>);
        }
    
        const notifications = this.props.data.notifications.current.filter(n => n.notificationType !== 'UPDATE_APP');

    return (<ScrollableContainer extraMargin={-20}>
      <Background />

      <div style={commonStyles.scrollBoxContainer}>
        <Background />

        <center>
            <RaisedButton label="Refresh" primary onClick={() => this.refresh()} />
        </center>
        
        <Divider />

        {this.state.errorRefreshing ? (<div>
          <p style={styles.errorText}>Unable to refresh notifications, check your wifi connection or try later.</p>
          <Divider />
        </div>) : <div />}

        {notifications && notifications.length ? (<div>
          {notifications.map(notification =>
            (<div key={notification.id}>
                <p>{notification.description}</p>
                <Divider />
              </div>))}
        </div>) : <div><p>You've got no notifications yet!</p></div> }
      </div>
    </ScrollableContainer>);
    }
};

export default withApollo(graphql(query)(Notifications));
