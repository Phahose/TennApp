import React from "react";
import { Link } from "react-router-dom";
import "../styles/styles.css"; // Styling (create this file in styles/)

const LandingPage = () => {
  return (
    <div className="landing-container">
      <h1>Welcome to OnCourt</h1>
      <p>Find and connect with tennis players near you!</p>
      <div className="button-container">
        <Link to="/login" className="btn">Login</Link>
        <Link to="/signup" className="btn">Sign Up</Link>
      </div>
      <div class="login-div" >
        <div>
            <h2>Hey...Who are you again? </h2>
        </div>
        <div class="login-container">      
            <form class="basicform" name="LoginForm" method="post">
                <label for="Email">Email</label><br />
                <input type="email" name="Email" required /><br />
                <label for="Password">Password</label><br />
                <input type="password" name="Password" required /><br />
                <input type="submit" name="Submit" value="Enter" />
            </form>
        </div>
 
    </div>  
    </div>
    
  );
};

export default LandingPage;
