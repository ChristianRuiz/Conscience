import React from 'react';

class ScrollableContainer extends React.Component {

  constructor(props) {
    super(props);

    this.state = {
      height: parseInt(window.innerHeight),
      margin: 100 + props.extraMargin
    };
  }

  render() {
    return (<div className="mainContent scrollableContainer" style={{ height: this.state.height - this.state.margin }}>
      {this.props.children}
    </div>);
  }

}

ScrollableContainer.defaultProps = {
  extraMargin: 0
};

ScrollableContainer.propTypes = {
  children: React.PropTypes.element.isRequired,
  extraMargin: React.PropTypes.number
};

export default ScrollableContainer;
