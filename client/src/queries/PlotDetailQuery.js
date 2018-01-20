import { gql } from 'react-apollo';

const query = gql`query GetPlotDetails($plotId:Int!) {
  plots {
    byId(id: $plotId) {
      id,
      name,
      description,
      characters {
        character {
          id,
          name,
          currentHost {
            id
            host {
              id
              account {
                id
                pictureUrl
              }
            }
          }
        },
        description
      }
      events {
        id
        description
        location
        hour
        minute
      }
    }
  }
}
      `;

export default query;
