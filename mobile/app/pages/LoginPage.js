import React from 'react';
import {
  View,
  StyleSheet
} from 'react-native';
import Login from '../components/login/Login';

const LoginPage = () =>
  <View style={styles.container}>
    <Login />
  </View>;

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center'
  }
});

export default LoginPage;

