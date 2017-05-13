import React from 'react';
import { Animated, View, Text, StyleSheet } from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { TabViewAnimated, TabBar } from 'react-native-tab-view';
import type { NavigationState } from 'react-native-tab-view/types';

import HostDetails from './host/HostDetails';
import PlotEvents from './host/PlotEvents';
import Memories from './host/Memories';
import Notifications from './host/Notifications';
import Debugger from './Debugger';

import SignalRService from './services/SignalRService';

type Route = {
  key: string,
  title: string,
  icon: string,
};

type State = NavigationState<Route>;

class HostTabs extends React.Component {
  static title = 'Bottom bar with indicator';
  static appbarElevation = 4;

  state: State = {
    index: 0,
    routes: [
      { key: '1', title: 'Me', icon: 'ios-body' },
      { key: '2', title: 'Events', icon: 'ios-calendar' },
      { key: '3', title: 'Mems', icon: 'ios-images' },
      { key: '4', title: 'Nots', icon: 'ios-notifications' },
      { key: '5', title: 'Debug', icon: 'ios-bug' }
    ]
  };

  _handleChangeTab = (index) => {
    this.setState({
      index
    });
  };

  _renderIndicator = (props) => {
    const { width, position } = props;

    const translateX = Animated.multiply(position, width);

    return (
      <Animated.View
        style={[styles.container, { width, transform: [{ translateX }] }]}
      >
        <View style={styles.indicator} />
      </Animated.View>
    );
  };

  _renderIcon = ({ route }) => <Icon name={route.icon} size={24} style={styles.icon} />;

  _renderBadge = ({ route }) => {
    if (route.key === '4') {
      return (
        <View style={styles.badge}>
          <Text style={styles.count}>1</Text>
        </View>
      );
    }
    return null;
  };

  _renderFooter = props => (
    <TabBar
      {...props}
      renderIcon={this._renderIcon}
      renderBadge={this._renderBadge}
      renderIndicator={this._renderIndicator}
      style={styles.tabbar}
      tabStyle={styles.tab}
    />
    );

  _renderScene = ({ route }) => {
    switch (route.key) {
      case '1':
        return (
          <View style={styles.container}>
            <HostDetails />
            <SignalRService />
          </View>
        );
      case '2':
        return (
          <PlotEvents />
        );
      case '3':
        return (
          <Memories />
        );
      case '4':
        return (
          <Notifications />
        );
      case '5':
        return (
          <Debugger />
        );
      default:
        return null;
    }
  };

  render() {
    return (<TabViewAnimated
        style={[styles.container, this.props.style]}
        navigationState={this.state}
        renderScene={this._renderScene}
        renderFooter={this._renderFooter}
        onRequestChangeTab={this._handleChangeTab}
      />);
  }
}

const styles = StyleSheet.create({
  container: {
    flex: 1
  },
  tabbar: {
    backgroundColor: '#222'
  },
  tab: {
    padding: 0
  },
  icon: {
    backgroundColor: 'transparent',
    color: 'white'
  },
  indicator: {
    flex: 1,
    backgroundColor: '#0084ff',
    margin: 4,
    borderRadius: 2
  },
  badge: {
    marginTop: 4,
    marginRight: 32,
    backgroundColor: '#f44336',
    height: 16,
    width: 16,
    borderRadius: 8,
    alignItems: 'center',
    justifyContent: 'center',
    elevation: 4
  },
  count: {
    color: '#fff',
    fontSize: 12,
    fontWeight: 'bold',
    marginTop: -2
  }
});

export default HostTabs;
