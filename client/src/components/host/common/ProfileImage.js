import React from 'react';

const styles = {
    image: {
      height: 110,
      width: 110,
      borderRadius: 55
    }
  };

class ProfileImage extends React.Component {
    state = {
        errorLoadingImage: false
    }
    
    render() {
        let source = '/content/images/host/defaultProfile.png';

        if (!this.state.errorLoadingImage && this.props.source) {
            source = this.props.source;
        }

        return (<img
            src={source} style={[styles.image, this.props.style]}
            onError={() => this.setState({ errorLoadingImage: true })}
        />);
    }
};

export default ProfileImage;
