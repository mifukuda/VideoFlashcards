import React from 'react';
import LoginBar from './LoginBar.js';
import LandingBar from './LandingBar.js';

export default function WelcomeScreen(props) {
    
    return(
        <div className="welcome-screen">
            <LandingBar/>
            <LoginBar setToken={props.setToken}/>
        </div>
    )
}

