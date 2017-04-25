import React from 'react';
import { graphql, gql } from 'react-apollo';


class RolesValidation extends React.Component {

  render() {
    if (this.props.data.loading) {
      return null;
    }

    if (this.props.allowed.length > 0 &&
     !this.props.data.accounts.getCurrent.roles
            .some(role =>
                this.props.allowed.some(r => r === role.name)
            )) {
      return null;
    }

    if (this.props.forbidden.length > 0 &&
     this.props.data.accounts.getCurrent.roles.length === 1 &&
     this.props.forbidden.some(r => r === this.props.data.accounts.getCurrent.roles[0])) {
      return null;
    }

    return this.props.children;
  }

}

RolesValidation.propTypes = {
  data: React.PropTypes.object.isRequired,
  allowed: React.PropTypes.array,
  forbidden: React.PropTypes.array,
  children: React.PropTypes.element.isRequired
};

RolesValidation.defaultProps = {
  allowed: [],
  forbidden: []
};

const query = gql`
query GetCurrentUserRoles {
  accounts
  {
    getCurrent
    {
      roles {
          name
      }
    }
  }
}
`;

export default graphql(query)(RolesValidation);
