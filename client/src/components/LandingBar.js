import React from 'react';
import Flashcard from './Flashcard';

export default function LandingBar() {
    return(
        <div className="landing-bar">
            <div className="landing-bar-info">
                <h1 className="landing-bar-title">Here's a website for creating video flashcards just like this one:</h1>
                <div className="landing-bar-card">
                    <Flashcard url="https://i.imgur.com/LOFimKX.mp4" title="Falco's Fullhop in the Corner" 
                        description="Watch for Falco's escape option in the corner, especially fullhop. Cover with knee or upair to set up for an early kill."/>
                </div>
            </div>
            <div className="landing-bar-animation">
                <img src={require('../images/walkingcat.gif')} alt="walkingcat.gif" className="landing-bar-gif"/>
            </div>
        </div>
    )
}
