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
  },
  policy: {
    marginTop: -60
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

        <Text style={styles.policy}>
          { `Aleph Quality Policy

Our Quality Policy reflects in our products and services, and is defined and driven by the following management principles and behaviors:

• Each and every Aleph employee shall build a mutually profitable relationship with our customers, ensuring their total and absolute satisfaction, through the understanding of their needs and their desires.

• We shall achieve our commitments for extreme quality, efficiency, and schedule

• Everyone at Aleph shall enhance the systematic research and use of best preventive practices at all levels and ensure reliable risk management

• We shall drive continuous improvement and innovation based on efficient business processes, well-defined measurements, best practices, and customer surveys

• Aleph's staff shall push their competencies, creativity, empowerment, and accountability through appropriate development programs and show strong management involvement and commitment

Aleph strives to be the best entertainment provider in the country, through continuous renovation and improvement of its products, to enhance the best possible experience for our beloved guest Their experience shall be so real, they will fully believe in Aleph alternate reality as their own.

Aleph goal is 100% customer satisfaction 100% of the time.`}
        </Text>
      </View>
    </ScrollView>);
  }
}

EmployeeDetails.propTypes = {
  data: React.PropTypes.object.isRequired
};

export default graphql(query)(EmployeeDetails);
