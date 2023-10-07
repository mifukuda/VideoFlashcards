import React, {useState} from 'react';

export default function Flashcard(props) {
    const [flip, setFlip] = useState(false);
    
    function handleClick() {
        setFlip(!flip);
    }
    
    return (
        <div className="flashcard">
            <div className={`flashcard-inner ${flip ? "flip" : ""}`}>
                <div className="flashcard-back">
                    <div className="flashcard-header">
                        <h2>{props.title}</h2>
                        </div>
                    <div className="flashcard-description">
                        <p>{props.description}</p>
                    </div>
                </div>
                <div className="flashcard-front">
                    <video draggable="false" playsInline loop hash="LOFimKX" autoPlay controls muted width="100%" className="flashcard-video">
                        <source type="video/mp4" src={props.url}/>
                    </video>
                </div>
            </div>
            <button className="flip-button" onClick={() => handleClick()}></button>
        </div>
    )
}