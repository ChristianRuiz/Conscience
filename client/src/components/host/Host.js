import React from 'react';

import { Tabs, Tab } from 'material-ui/Tabs';
import IconAccountCircle from 'material-ui/svg-icons/action/account-circle';
import IconAccessibility from 'material-ui/svg-icons/action/accessibility';
import IconAlarm from 'material-ui/svg-icons/action/alarm';
import IconNotification from 'material-ui/svg-icons/social/notifications';

import Background from './common/Background';

import Me from './me/me';
import Stats from './stats/stats';
import Events from './events/events';
import Notifications from './notifications/notifications';

import hoststyles from '../../styles/components/host/host.css';

const tabsContent = {
  1: Me,
  2: Stats,
  3: Events,
  4: Notifications
};

class Host extends React.Component {
  state = {
    selectedTab: 1
  }

  render() {
    return (<div>
      <div className="hostcontainer">
        <div className="host-tabsContent">
          {
            React.createElement(tabsContent[this.state.selectedTab])
          }
        </div>

        <Tabs
          className="host-tabs"
          onChange={(value) => this.setState({selectedTab: value})}
          tabItemContainerStyle={{backgroundColor: '#222222'}}
          inkBarStyle={{backgroundColor: '#0084FF', height: 4}}
        >
          <Tab
            value={1}
            icon={<IconAccountCircle />}
            label="ME"
          />
          <Tab
            value={2}
            icon={<IconAccessibility />}
            label="STATS"
          />
          <Tab
            value={3}
            icon={<IconAlarm />}
            label="EVENTS"
          />
          <Tab
            value={4}
            icon={<IconNotification />}
            label="NOTS"
          />
        </Tabs>
      </div>
    </div>
    );
  }
}

export default Host;
