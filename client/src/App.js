import {createContext, useState} from 'react';
import './App.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import { WelcomeScreen } from './components';

function App() {
  const [token, setToken] = useState("");
  const TokenContext = createContext();

  return (
    <TokenContext.Provider value={token}>
      <div className="App">
        <WelcomeScreen setToken={setToken}/>
      </div>
    </TokenContext.Provider>
  );
}

export default App;
