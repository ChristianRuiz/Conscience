import React from 'react';

class ScrollableContainer extends React.Component {

  constructor(props) {
    super(props);

    this.state = {
      height: props.parentHeight,
      margin: 100 + props.extraMargin
    };
  }

  componentWillReceiveProps(props) {
    if (props.parentHeight && this.state.height !== props.parentHeight) {
      this.setState({ height: props.parentHeight });
    }
  }

  render() {
    return (<div className="mainContent scrollableContainer" style={{ height: this.state.height - this.state.margin }}>
      {this.props.children}
    </div>);
  }

}

ScrollableContainer.defaultProps = {
  extraMargin: 0,
  parentHeight: parseInt(window.innerHeight)
};

ScrollableContainer.propTypes = {
  children: React.PropTypes.element.isRequired,
  extraMargin: React.PropTypes.number,
  parentHeight: React.PropTypes.number
};

export default ScrollableContainer;
