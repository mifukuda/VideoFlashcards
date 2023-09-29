import React, {useState} from 'react';
import Sidebar from './Sidebar';

export default function LandingBar() {
    const [showSidebar, setShowSidebar] = useState(false);
    
    return(
        <div className="home-screen">
            <Sidebar showSidebar={showSidebar} setShowSidebar={setShowSidebar}/>
        </div>
    )
}