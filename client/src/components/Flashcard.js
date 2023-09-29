import React from 'react';

export default function Flashcard(props) {
    
    return (
        <div className="flashcard">
            <div className="flashcard-header">
                <h2>{props.title}</h2>
            </div>
            <video draggable="false" playsInline loop hash="LOFimKX" autoPlay controls muted width="100%" className="flashcard-video">
                <source type="video/mp4" src={props.url}/>
            </video>
            <div className="flashcard-description">
                <p>{props.description}</p>
            </div>
        </div>
    )
}