import React from 'react';
import { withRouter } from 'react-router-dom';
import { graphql, gql } from 'react-apollo';

import PictureDescriptionBox from '../common/PictureDescriptionBox';

class PlotDetail extends React.Component {
  render() {
    if (this.props.data.loading) {
      return (<div>Loading...</div>);
    }

    const plot = this.props.data.plots.byId;

    return (<div>
      <h2>{plot.name}</h2>
      <p>{plot.description}</p>

      <div className="flexColumn marginTop">
        {plot.characters.map(c =>
          <PictureDescriptionBox
            key={c.character.id}
            pictureUrl={c.character.currentHost ? c.character.currentHost.host.account.pictureUrl : ''}
            title={c.character.name}
            link={`/character-detail/${c.character.id}`}
            description={c.description}
          />)}
      </div>

      <div className="flexColumn marginTop">
        {plot.events.map(e =>
          <div key={e.id}>
            <h3>Event: {e.description}</h3>
            <p>{e.location}</p>
            <p>{`0${e.hour}`.slice(-2)}:{`0${e.minute}`.slice(-2)}</p>
          </div>)}
      </div>
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

export default withRouter(graphql(query)(PlotDetail));
