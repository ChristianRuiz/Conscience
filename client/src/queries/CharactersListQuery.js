import { gql } from 'react-apollo';

const query = gql`query GetCharacters {
  characters {
    all {
      id,
      name
      currentHost {
        id
        host {
          id
          account {
            id,
            userName
          }
        }
      }
    }
  }
}
      `;

export default query;
