var path = require('path');

module.exports = {
  entry: { 
    'login': './src/pages/login.js',
    'employees-list': './src/pages/employees-list.js' 
  },
  output: {
    filename: '../../Scripts/conscience/[name].js',
    path: path.resolve(__dirname, 'dist')
  },
  module: {
    loaders: [
      {
        test: /.js?$/,
        loader: 'babel-loader',
        exclude: /node_modules/,
        query: {
          presets: ['es2015', "stage-1", "stage-2", 'react'],
          plugins: [ path.resolve(__dirname, 'src/plugins/babelRelayPlugin') ]
        }
      }
    ]
  }
};