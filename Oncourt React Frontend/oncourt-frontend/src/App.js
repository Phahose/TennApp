//import logo from './logo.svg';
import "./styles/App.css";
import API_BASE_URL from "./config/config";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import axios from "axios";
import LandingPage from "./pages/Index"; 
import Login from "./pages/Login";
import Signup from "./pages/Signup";

axios.get(API_BASE_URL).then(response => {
  console.log(response.data);
}).catch(error => {
  console.error("Error fetching courts:", error);
});


function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<LandingPage />} />
        <Route path="/login" element={<Login />} />
        <Route path="/signup" element={<Signup />} />
      </Routes>
    </Router>
  );
}

export default App;
