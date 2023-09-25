import React from 'react';

export default function LandingBar() {
    return(
        <div className="landing-bar">
            <div className="landing-bar-info">
                Landing bar 
            </div>
            <div className="landing-bar-animation">
                <img src={require('../images/walkingcat.gif')} alt="walkingcat.gif" className="landing-bar-gif"/>
            </div>
        </div>
    )
}
