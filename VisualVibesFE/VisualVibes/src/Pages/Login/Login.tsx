import React from 'react'
import "./Login.css"
import { Helmet } from 'react-helmet-async';
import { useNavigate } from 'react-router-dom';

const Login = () => {

    const navigate = useNavigate();

    const handleSignup = () => {
        navigate("/signup");
    }

  return (
    <>
        <Helmet>
            <title>Login</title>
        </Helmet>
        <div className="login">
            <div className="loginWrapper">
                <div className="loginLeft">
                    <h3 className="loginLogo">VisualVibes</h3>
                    <span className="loginDescription">
                        Connect with your friends
                    </span>
                </div>
                <div className="loginRight">
                    <div className="loginBox">
                        <input placeholder="Email" className="loginInput"/>
                        <input placeholder="Password" className="loginInput"/>
                        <button className="loginForgotPassword">Forgot Password?</button>
                        <button className="loginButton">Log In</button>
                        <button className="loginRegistrationButton" onClick={handleSignup}>
                            Don't have an account? Sign up
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </>
  )
}

export default Login;