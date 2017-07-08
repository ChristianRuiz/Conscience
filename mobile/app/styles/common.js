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
    minHeight: 600,
    padding: 30
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
    fontWeight: 'bold',
    color: '#26D639'
  },
  bold: {
    fontWeight: 'bold'
  },
  text: {
    fontFamily: 'DejaVuSans',
    color: 'white',
    textAlign: 'justify'
  },
  textInput: {
    fontFamily: 'DejaVuSans',
    color: 'white'
  },
  button: {
    fontFamily: 'DejaVuSans',
    color: 'pink'
  }
});

export default styles;
