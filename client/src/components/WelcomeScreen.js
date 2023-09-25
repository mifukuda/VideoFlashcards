import React from 'react';
import LoginBar from './LoginBar.js';

export default function WelcomeScreen(props) {
    return(
        <div className="welcome-screen">
            <div className="landing-screen">
                Landing
            </div>
            <LoginBar setToken={props.setToken}/>
        </div>
    )
}

