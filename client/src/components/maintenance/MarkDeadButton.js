import React from 'react';
import { graphql, gql } from 'react-apollo';

class MarkDeadButton extends React.Component {
  markDead() {
    const shouldCall = confirm('Are you sure you want to call the host?');
    if (shouldCall) {
      this.props.mutate({ variables: { hostId: this.props.hostId } });
    }
  }

  render() {
    return <button className="linkButton" onClick={() => this.callHost()}><h3>Call host</h3></button>;
  }
}

MarkDeadButton.propTypes = {
  hostId: React.PropTypes.number.isRequired,
  mutate: React.PropTypes.func.isRequired
};

const mutation = gql`
mutation CallHost($hostId:Int!) {
  hosts {
    call(hostId:$hostId){
      id
    }
  }
}
`;

export default graphql(mutation)(MarkDeadButton);
