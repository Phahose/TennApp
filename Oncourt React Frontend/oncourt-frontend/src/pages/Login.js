import React from "react";
import "../styles/styles.css"; // Styling (create this file in styles/)

const Login = () => {
  return (
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
  );
};

export default Login;
