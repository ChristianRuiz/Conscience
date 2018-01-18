import React from 'react';
import { withRouter } from 'react-router-dom';
import { graphql, gql } from 'react-apollo';

class CriticalFailureCheck extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      hasFailure: false,
      fixed: false
    };

    this.errors = [
      'Connection 3435FT# Failed, please check the page 20',
      'Subtongue articulation mechanism failure. Please check the page 36',
      'Walking gyroscope malfunction: please check the page 56',
      'Walking mechanism failed: please check the page 141',
      'Sight critical warning, eyes device Kyneticon (TM) malfunction, please check page 189',
      'Language Intel Processor critical failure: please check the page 84',
      'Remote communication failed, please check page 93',
      'Perception protocol malfunction, please check page 113',
      'Circular movement error above standardized limits, please check page 58',
      'Happiness protocol 482H56 malfunction, check page 31',
      'Sadness protocol 398J16 malfunction, check page 81',
      'Anger protocol 277R08 malfunction, check page 161',
      'Stupor protocol 012H66 malfunction, check page 184',
      'Fear protocol 666A27 malfunction, check page 129',
      'Error 124: malfunction chipset JHT689, please check the page 24',
      'Body connection recognition protocol failed, please check the page 73',
      'Handshaking communication IP protocol alert, please check page 166',
      'Speed movement failure, please check page 43',
      'Volume level tuning malfunction, please check page 150',
      'Aiming chipset malfunction, check page 14',
      'Reaction movement chipset failure, go to page 177',
      'Simulated breathing vent malfuncion, go to page 47',
      'Simulated sufference algorithm failure, go to page 12',
      'Simulated tears chipset critical failure, go to page 29',
      'Low coolant level, go to page 38',
      'Eye-hands coordination malfuncition, check page 198'
    ];
  }

  componentWillReceiveProps(props) {
    if (!props.data.loading && props.data.hosts.hasFailure) {
      this.setState({ hasFailure: true });
    }
  }

  render() {
    if (this.props.data.loading) {
      return (<div />);
    }

    if (this.state.hasFailure && !this.state.fixed) {
      // TODO: Start alarm sound

      const firstError = this.errors.splice(Math.floor(Math.random() * this.errors.length), 1);

      alert(firstError);

      const secondError = this.errors.splice(Math.floor(Math.random() * this.errors.length), 1);

      alert(secondError);

      // TODO: End alarm sound

      this.setState({ fixed: true });
    }

    return <div />;
  }
}

CriticalFailureCheck.propTypes = {
  data: React.PropTypes.object.isRequired
};

const query = gql`query HasFailure($hostId:Int!) {
  hosts {
    hasFailure(hostId: $hostId)
  }
}
      `;


export default withRouter(graphql(query)(CriticalFailureCheck));
