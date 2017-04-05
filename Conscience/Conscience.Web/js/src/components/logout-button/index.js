import React from 'react';
import { graphql, gql, compose } from 'react-apollo';
import RaisedButton from 'material-ui/RaisedButton';

class LogoutButton extends React.Component {
    constructor(props) {
        super(props);
        
        this._doLogout = this._doLogout.bind(this);
    }
    
    render() {
        return <div>{!this.props.data.loading && <label>{this.props.data.accounts.getCurrent.userName}</label>} 
        <RaisedButton label="Logout" primary={true} onClick={this._doLogout} /></div>;
    }

    _doLogout() {
        this.props.mutate()
        .then(({ data }) => {
            document.location.href = "/Login";
        }).catch((error) => {
            console.warn("Unable to logout");
        });
    }
}

const mutation = gql`
mutation Logout {
  accounts
  {
    logout
    {
      id
    }
  }
}
`;

const query = gql`
query GetCurrentUser {
  accounts
  {
    getCurrent
    {
      userName
    }
  }
}
`;

export default compose(graphql(mutation),
                        graphql(query))(LogoutButton);