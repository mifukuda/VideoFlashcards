import './App.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import { BrowserRouter, Routes, Route } from "react-router-dom";
import { WelcomeScreen, RegistrationScreen, HomeScreen } from './components';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<WelcomeScreen/>}/>
        <Route path="/register" element={<RegistrationScreen/>}/>
        <Route path="/home" element={<HomeScreen/>}/>
      </Routes>
    </BrowserRouter>
  );
}

export default App;
