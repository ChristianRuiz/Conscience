import React from 'react';
import { graphql, gql } from 'react-apollo';

class MarkDeadButton extends React.Component {
  markDead() {
    const shouldMark = confirm('Mark the host as injured?');
    if (shouldMark) {
      this.props.mutate({ variables: { hostId: this.props.hostId } });
    }
  }

  render() {
    return <button className="linkButton" onClick={() => this.markDead()}><h3>HURT</h3></button>;
  }
}

MarkDeadButton.propTypes = {
  hostId: React.PropTypes.number.isRequired,
  mutate: React.PropTypes.func.isRequired
};

const mutation = gql`
mutation MarkAsDead($hostId:Int!) {
  hosts {
    changeStatusToHost(hostId:$hostId, status: HURT){
      id
      status
    }
  }
}
`;

export default graphql(mutation)(MarkDeadButton);
