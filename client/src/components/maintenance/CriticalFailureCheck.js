import React from 'react';
import { withRouter } from 'react-router-dom';
import { graphql, gql } from 'react-apollo';
import Modal from 'react-modal';
import RaisedButton from 'material-ui/RaisedButton';

class CriticalFailureCheck extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      hasFailure: false,
      fixed1: false,
      fixed2: false
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

  secondErrorFixed() {
    window.maintenanceAudio.pause();
    window.maintenanceAudio = null;
    this.setState({ fixed2: true });
  }

  render() {
    if (this.props.data.loading) {
      return (<div />);
    }

    const modalHeight = 150;
    const modalWidth = 400;
    const modalStyle = {
      content: {
        height: modalHeight,
        width: modalWidth,
        top: (window.innerHeight - modalHeight) / 2,
        left: (window.innerWidth - modalWidth) / 2
      }
    };

    const pStyle = {
      height: 80
    };

    if (this.state.hasFailure && !this.state.fixed1) {
      if (!window.maintenanceAudio) {
        window.maintenanceAudio = new Audio('/Content/audio/maintenancealarm.mp3');
        window.maintenanceAudio.loop = true;
        window.maintenanceAudio.play();
      }

      const firstError = this.errors.splice(Math.floor(Math.random() * this.errors.length), 1);

      return (<Modal isOpen style={modalStyle} ariaHideApp={false}>
        <p style={pStyle}>{firstError}</p>
        <RaisedButton label="Fixed" primary onClick={() => this.setState({ fixed1: true })} />
      </Modal>);
    }

    if (this.state.hasFailure && !this.state.fixed2) {

      const secondError = this.errors.splice(Math.floor(Math.random() * this.errors.length), 1);

      return (<Modal isOpen style={modalStyle} ariaHideApp={false}>
        <p style={pStyle}>{secondError}</p>
        <RaisedButton label="Fixed" primary onClick={() => this.secondErrorFixed()} />
      </Modal>);
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


export default withRouter(graphql(query, { options: { fetchPolicy: 'network-only' } })(CriticalFailureCheck));
