import React from 'react';
import { Link, withRouter } from 'react-router-dom';
import { graphql } from 'react-apollo';

import RolesValidation from '../common/RolesValidation';
import Roles from '../../enums/roles';

import query from '../../queries/PlotDetailQuery';

import PictureDescriptionBox from '../common/PictureDescriptionBox';

class PlotDetail extends React.Component {
  render() {
    if (this.props.data.loading) {
      return (<div>Loading...</div>);
    }

    const plot = this.props.data.plots.byId;

    return (<div>
      <div className="flex">
        <h2 className="flexStretch">{plot.name}</h2>

        <RolesValidation allowed={[Roles.CompanyPlotEditor, Roles.Admin]}>
          <Link style={{ marginRight: 20 }} to={`/plot-edit/${plot.id}`} ><h3>Edit</h3></Link>
        </RolesValidation>
      </div>

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

      <p>Writer: {plot.writer.name}</p>
    </div>);
  }
}

PlotDetail.propTypes = {
  data: React.PropTypes.object.isRequired
};

export default withRouter(graphql(query, { options: { fetchPolicy: 'network-only' } })(PlotDetail));
