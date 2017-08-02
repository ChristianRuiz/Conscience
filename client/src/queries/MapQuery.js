import { gql } from 'react-apollo';

const query = gql`query GetAllAccountsForMap
        {
          accounts {
            all {
              id
              userName
              pictureUrl
              device {
                id
                currentLocation {
                  id,
                  latitude
                  longitude
                }
                online
              },
              host {
                id,
                currentCharacter {
                  id,
                  character {
                    id,
                    name
                  }
                }
              },
              employee {
                id,
                name
              }
            }
          }
        }
`;

export default query;
