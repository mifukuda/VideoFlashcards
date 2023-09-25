import {createContext, useState} from 'react';
import './App.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import { BrowserRouter, Routes, Route } from "react-router-dom";
import { WelcomeScreen, RegistrationScreen, HomeScreen } from './components';

function App() {
  const [token, setToken] = useState("");
  const TokenContext = createContext();

  return (
    <TokenContext.Provider value={token}>
      <BrowserRouter>
			  <Routes>
				  <Route path="/" element={<WelcomeScreen setToken={setToken}/>}/>
          <Route path="/register" element={<RegistrationScreen setToken={setToken}/>}/>
          <Route path="/home" element={<HomeScreen/>}/>
			  </Routes>
		  </BrowserRouter>
    </TokenContext.Provider>
  );
}

export default App;
