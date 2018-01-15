import { gql } from 'react-apollo';

const query = gql`query GetHostDetails($hostId:Int!) {
  hosts {
    byId(id:$hostId)
    {
      id,
      account {
        id,
        userName
      },
      currentCharacter {
        id,
        character {
          id,
          name
        }
      },
      stats {
        id,
        name,
        value
      }
    }
  }
}
      `;

export default query;
