import { gql } from 'react-apollo';

const query = gql`query GetCharacterDetails($characterId: Int!) {
  characters {
    byId(id: $characterId) {
      id
      name
      age
      gender
      story
      narrativeFunction
      memories {
        id
        description
      }
      triggers {
        id
        description
      }
      plots {
        id
        plot {
          id
          name
          description
        }
        description
      }
      plotEvents {
        id
        plot {
          id
          name
        }
        description
        location
        hour
        minute
      }
      relations {
        id
        description
        character {
          id
          name
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
        }
        inverseRelation {
          id
          description
        }
      }
      currentHost {
        id
        host {
          id
          account {
            id
            userName
            device {
              id
              lastConnection
              online
              batteryLevel
            }
          }
          coreMemory1 {
            id
            audio {
              transcription
            }
            locked
          }
          coreMemory2 {
            id
            audio {
              transcription
            }
            locked
          }
          coreMemory3 {
            id
            audio {
              transcription
            }
            locked
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
}
      `;

export default query;
