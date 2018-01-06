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
        status
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
      employee {
        id
        name
        department
      }
    }
  }
  notifications {
    current {
      id
      description
      timeStamp
      read
      notificationType
      employee {
        id
        name
        department
      }
      audio {
        id
        transcription
        path
      }
    }
  }
}
`;

export default query;
