import React from 'react';
import {
  View,
  ScrollView,
  Image,
  StyleSheet
} from 'react-native';
import { graphql } from 'react-apollo';
import Spinner from 'react-native-loading-spinner-overlay';

import Background from '../common/Background';
import Text from '../common/Text';
import ProfileImage from '../common/ProfileImage';
import ImageUploader from '../common/ImageUploader';

import commonStyles from '../../styles/common';

import query from '../../queries/HostDetailQuery';

const styles = StyleSheet.create({
  image: {
    position: 'absolute',
    top: 40,
    left: 20,
    height: 110,
    width: 110,
    borderRadius: 55
  },
  name: {
    position: 'absolute',
    top: 42,
    left: 160,
    color: '#34FFFC',
    fontWeight: 'bold',
    fontSize: 16,
    width: 145
  },
  department: {
    position: 'absolute',
    top: 80,
    left: 160,
    width: 145,
    fontSize: 16,
    fontWeight: 'bold',
    lineHeight: 25
  }
});

class EmployeeDetails extends React.Component {
  render() {
    if (this.props.data.loading || !this.props.data.accounts || !this.props.data.accounts.current) {
      return (<View style={commonStyles.container}>
        <Background />
        <Spinner visible />
      </View>);
    }

    const account = this.props.data.accounts.current;
    const employee = account.employee;

    return (<ScrollView>
      <Background />

      <View style={commonStyles.scrollBoxContainer}>

        <ProfileImage style={styles.image} source={account.pictureUrl} />

        <Image source={require('../../img/cardEmployee.png')} style={{ height: 234, width: 299, marginLeft: -10 }} />

        <ImageUploader style={styles.image} accountId={account.id} />

        <Text style={styles.name} numberOfLines={1}>{employee.name}</Text>
        <Text style={styles.department} numberOfLines={3}>{employee.department}</Text>

      </View>
    </ScrollView>);
  }
}

EmployeeDetails.propTypes = {
  data: React.PropTypes.object.isRequired
};

export default graphql(query)(EmployeeDetails);
