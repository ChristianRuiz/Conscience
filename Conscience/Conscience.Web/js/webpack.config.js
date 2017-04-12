const path = require('path');

module.exports = {
  entry: {
    login: './src/pages/login.js',
    main: './src/pages/main.js'
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
