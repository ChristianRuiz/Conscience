import {
  StyleSheet
} from 'react-native';

const styles = StyleSheet.create({
  container: {
    position: 'absolute',
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    top: 0,
    left: 0,
    bottom: 0,
    right: 0
  },
  scrollBoxContainer: {
    flex: 1,
    minHeight: 600
  },
  center: {
    justifyContent: 'center',
    alignItems: 'center'
  },
  p: {
    marginTop: 10
  },
  h3: {
    fontSize: 16,
    marginTop: 20,
    marginBottom: 10,
    fontWeight: 'bold'
  },
  bold: {
    fontWeight: 'bold'
  },
  text: {
    color: 'white'
  },
  textInput: {
    color: 'white'
  },
  button: {
    color: 'pink'
  }
});

export default styles;
