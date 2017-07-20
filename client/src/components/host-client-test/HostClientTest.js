import React from 'react';
import { withRouter } from 'react-router-dom';
import { graphql, gql } from 'react-apollo';
import RaisedButton from 'material-ui/RaisedButton';

class HostClientTest extends React.Component {
  constructor(props) {
    super(props);

    this._updateLocation = this._updateLocation.bind(this);
  }

  _updateLocation() {
    const location = { Latitude: 37.048601, Longitude: -2.4216117, TimeStamp: new Date().toISOString() };

    let random = Math.random() / 1000;

    location.Latitude += random;

    random = Math.random() / 1000;

    location.Longitude += random;

    const update = {
      deviceId: 'DevTest',
      batteryLevel: 91,
      charging: false,
      locations: [location]
    };

      fetch('/api/Notifications', {
        method: 'POST',
        body: JSON.stringify(update),
        credentials: 'same-origin'
      }).then(() => {
        console.log('Location sent');
      })
      .catch((e) => { console.log(`Unable to to send location updates to the server (ajax): ${e}`); });
  }

  render() {
    return (<div>
      <RaisedButton label="Update Location" onClick={this._updateLocation} />
    </div>);
  }
}

HostClientTest.propTypes = {
  data: React.PropTypes.object.isRequired
};

const query = gql`
query GetCurrentUser {
  accounts
  {
    current
    {
      id,
      userName
    }
  }
}
`;

export default withRouter(graphql(query)(HostClientTest));
