import { gql } from 'react-apollo';

const query = gql`query GetNotifications {
  notifications {
    current {
      id
      read
    }
  }
}
      `;

export default query;
