import React from 'react';

class ScrollableContainer extends React.Component {

  constructor(props) {
    super(props);

    this.state = {
      height: parseInt(window.innerHeight)
    };
  }

  render() {
    return (<div className="mainContent scrollableContainer" style={{ height: this.state.height - 100 }}>
      {this.props.children}
    </div>);
  }

}

ScrollableContainer.propTypes = {
  children: React.PropTypes.element.isRequired
};

export default ScrollableContainer;
