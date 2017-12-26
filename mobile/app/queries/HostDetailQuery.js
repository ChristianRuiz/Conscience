import { gql } from 'react-apollo';

const query = gql`{
  accounts {
    current {
      id
      userName
      pictureUrl
      device {
        lastConnection
        online
        batteryLevel
      }
      host {
        id
        currentCharacter {
          id
          assignedOn
          character {
            id
            name
            age
            gender
            story
            narrativeFunction
            triggers {
              id
              description
            }
            plots {
              id
              plot {
                name
                description
              }
              description
            }
            memories {
              id
              description
            }
            relations {
              id
              description
              character {
                name,
                currentHost {
                  host {
                    account {
                      pictureUrl
                    }
                  }
                }
              }
            }
            plotEvents {
              id
              plot {
                name
              }
              description
              location
              hour
              minute
            }
          }
        }
        stats {
          id
          name
          value
        }
      }
    }
  }
}
`;

export default query;
