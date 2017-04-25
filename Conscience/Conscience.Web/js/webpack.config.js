const path = require('path');
require('babel-polyfill');

module.exports = {
  entry: {
    login: ['babel-polyfill', './src/pages/login.js'],
    main: ['babel-polyfill', './src/pages/main.js']
  },
  output: {
    filename: '../../Scripts/conscience/[name].js',
    path: path.resolve(__dirname, 'dist')
  },
  devtool: '#eval-source-map',
  module: {
    loaders: [
      {
        test: /.js?$/,
        loader: 'babel-loader',
        exclude: /node_modules/,
        query: {
          presets: ['es2015', 'stage-0', 'stage-1', 'stage-2', 'react']
        }
      },
      {
        test: require.resolve('jquery'),
        loader: 'expose-loader?jQuery!expose-loader?$'
      }
    ]
  }
};
