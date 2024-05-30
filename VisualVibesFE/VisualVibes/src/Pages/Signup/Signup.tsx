import React from 'react'
import { Helmet } from 'react-helmet-async';
import { useNavigate } from 'react-router-dom';

const Signup = () => {

    const navigate = useNavigate();

    const handleLogin = () => {
        navigate("/login");
    }

  return (
    <>
    <Helmet>
        <title>SignUp</title>
    </Helmet>
    <div className="signup">
        Signup page
    </div>
    <button className="signupLoginButton" onClick={handleLogin}>
        Alreay have an account? Log in
    </button>
    </>
  )
}

export default Signup;