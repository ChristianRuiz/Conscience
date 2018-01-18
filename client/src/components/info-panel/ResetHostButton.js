import React from 'react';
import { graphql, gql } from 'react-apollo';

class ResetHostButton extends React.Component {
  callHost() {
    const shouldCall = confirm('Are you sure you want to reset the host?');
    if (shouldCall) {
      this.props.mutate({ variables: { hostId: this.props.hostId } });
    }
    return false;
  }

  render() {
    return <button className="linkButton" onClick={() => this.callHost()}><h3>Reset host</h3></button>;
  }
}

ResetHostButton.propTypes = {
  hostId: React.PropTypes.number.isRequired,
  mutate: React.PropTypes.func.isRequired
};

const mutation = gql`
mutation ResetHostButton($hostId:Int!) {
  hosts {
    reset(hostId:$hostId){
      id
    }
  }
}
`;

export default graphql(mutation)(ResetHostButton);
