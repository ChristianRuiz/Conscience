import { gql } from 'react-apollo';

const query = gql`{
  accounts {
    current {
      id
      host {
        id
        account {
          userName
          pictureUrl
          device {
            lastConnection
            online
            batteryLevel
          }
        }
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
              characters {
                id
                name
              }
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
