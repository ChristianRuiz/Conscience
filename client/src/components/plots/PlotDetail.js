import React from 'react';
import { Link, withRouter } from 'react-router-dom';
import { graphql, gql } from 'react-apollo';

class PlotDetail extends React.Component {
  render() {
    if (this.props.data.loading) {
      return (<div>Loading...</div>);
    }

    const plot = this.props.data.plots.byId;

    return (<div>
      <h2>{plot.name}</h2>
      <p>{plot.description}</p>

      <ul>
        {plot.characters.map(c =>
          <li key={c.character.id}>
            <p>
              <Link to={`/character-detail/${c.character.currentHost.host.id}`} ><b>{c.character.name}: </b></Link>
              {c.description}
            </p>
          </li>)}
      </ul>
    </div>);
  }
}

PlotDetail.propTypes = {
  data: React.PropTypes.object.isRequired
};

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
    }
  }
}
      `;

export default withRouter(graphql(query)(PlotDetail));
