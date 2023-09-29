import React from 'react';

export default function Sidebar(props) {

    function handleClick() {
        props.setShowSidebar(!props.showSidebar);
    }

    let style = {
        width: "4%"
    }

    if(props.showSidebar) {
        style = {
            width: "20%"
        }
    }

    let body = <a href={() => false} class="sidebar-button" onClick={() => handleClick()}>&#8801;</a>

    return (
        <div className="sidebar" style={style}>
            {/* <a href={() => false} class="sidebar-button" onClick={() => handleClick()}>&times;</a> */}
            {body}
        </div>
    )
}