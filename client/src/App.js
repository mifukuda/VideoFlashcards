import {createContext, useState} from 'react';
import './App.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import { BrowserRouter, Routes, Route } from "react-router-dom";
import { WelcomeScreen } from './components';

function App() {
  const [token, setToken] = useState("");
  const TokenContext = createContext();

  return (
    <TokenContext.Provider value={token}>
      <BrowserRouter>
			  <Routes>
				  <Route path="/" element={<WelcomeScreen setToken={setToken}/>}/>
			  </Routes>
		  </BrowserRouter>
    </TokenContext.Provider>
  );
}

export default App;
