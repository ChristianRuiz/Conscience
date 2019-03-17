import React from 'react';

const ImageRepeater = ({ src, srcSize, style }) => {
    const backgroundStyle = {
        ...style,
        marginTop: '-6px',
        backgroundImage: `url(${src})`,
        backgroundSize: `${srcSize.width}px ${srcSize.height}px`,
        backgroundRepeat: 'repeat',
    };

    return <div style={backgroundStyle} />;
};

export default ImageRepeater;
