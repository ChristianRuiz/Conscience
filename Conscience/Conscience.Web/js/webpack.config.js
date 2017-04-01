var path = require('path');

module.exports = {
  entry: [ './src/pages/login.js' ],
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
          presets: ['es2015', "stage-1", "stage-2", 'react']
        }
      }
    ]
  }
};