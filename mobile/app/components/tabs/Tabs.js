import React from 'react';
import { Animated, View, StyleSheet } from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { TabViewAnimated, TabBar } from 'react-native-tab-view';
import Spinner from 'react-native-loading-spinner-overlay';

import { withApollo, graphql } from 'react-apollo';

import Text from '../common/Text';

import HostTabs from './HostTabs';
import EmployeeTabs from './EmployeeTabs';
import Background from '../common/Background';

import NotificationsService from '../../services/NotificationsService';
import Constants from '../../constants';

import commonStyles from '../../styles/common';

import query from '../../queries/HostDetailQuery';


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
    marginRight: 16,
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

class Tabs extends React.Component {
  static title = 'Conscience';
  static appbarElevation = 4;

  state = {
    index: 0,
    notificationsTab: '0',
    notificationsCount: 0,
    routes: [],
    renderScene: () => null
  };

  _handleChangeTab = (index) => {
    this.setState({
      index,
      notificationsCount: (index + 1).toString() === this.state.notificationsTab ? 0
                                                : this.state.notificationsCount
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
    if (route.key === this.state.notificationsTab && this.state.notificationsCount) {
      return (
        <View style={styles.badge}>
          <Text style={styles.count}>{this.state.notificationsCount}</Text>
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

  componentWillReceiveProps(props) {
    if (!props.data.loading) {
      if (this.state.routes.length === 0) {
        if (props.data.accounts.current.employee) {
          this.setState({ routes: EmployeeTabs.routes, renderScene: EmployeeTabs.renderScene, notificationsTab: EmployeeTabs.notificationsTab });
        } else {
          this.setState({ routes: HostTabs.routes, renderScene: HostTabs.renderScene, notificationsTab: HostTabs.notificationsTab });
        }
      }
      this.setState({
        notificationsCount: props.data.notifications.current.filter(n => !n.read).length + this.state.notificationsCount
      });
    }
  }

  componentDidMount() {
    if (!global.notificationsService && Constants.SERVER_URL.indexOf('azurewebsites') === -1) {
      global.notificationsService = new NotificationsService(this.props.client,
            navigator, global.audioService);
    }
  }

  render() {
    if (this.props.data.loading || !this.state.routes.length) {
      return (<View style={commonStyles.container}>
        <Background />
        <Text>Loading...</Text>
        <Spinner visible />
      </View>);
    }

    return (<TabViewAnimated
      style={[styles.container, this.props.style]}
      navigationState={this.state}
      renderScene={this.state.renderScene}
      renderFooter={this._renderFooter}
      onRequestChangeTab={this._handleChangeTab}
    />);
  }
}

Tabs.propTypes = {
  client: React.PropTypes.object.isRequired,
  data: React.PropTypes.object.isRequired
};

export default withApollo(graphql(query)(Tabs));
