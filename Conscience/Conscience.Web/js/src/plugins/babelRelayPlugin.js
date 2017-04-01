var getBabelRelayPlugin = require('babel-relay-plugin');

var schemaData = require('../data/schema.json');

module.exports = getBabelRelayPlugin(schemaData);