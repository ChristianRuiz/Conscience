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
                batteryLevel
                batteryStatus
                lastConnection
                currentLocation {
                  id,
                  latitude
                  longitude
                }
              },
              host {
                id,
                status,
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
