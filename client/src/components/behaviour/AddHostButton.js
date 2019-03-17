import React from 'react';
import { graphql, gql } from 'react-apollo';

class AddHostButton extends React.Component {
  addHost() {
    const accountName = prompt('What\'s the login name?');

    if (accountName) {
        const shouldAdd = confirm(`Are you sure to add a new host '#${accountName}'?`);
        if (shouldAdd) {
            this.props.mutate({ variables: { accountName } }).then(() => {
                if (this.props.onHostAdded) {
                    this.props.onHostAdded();
                }
            });
        }
    }
  }

  render() {
    return <button className="linkButton" onClick={() => this.addHost()}><h3>ADD HOST</h3></button>;
  }
}

AddHostButton.propTypes = {
  onHostAdded: React.PropTypes.func,
  mutate: React.PropTypes.func.isRequired
};

const mutation = gql`
mutation AddHost ($accountName:String!) {
    hosts {
      addHost(accountName: $accountName) {
        id
      }
    }
  }
`;

export default graphql(mutation)(AddHostButton);
