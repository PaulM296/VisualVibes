import React from 'react'
import './UserBlocked.css'
import { Helmet } from 'react-helmet-async'
import { Link } from 'react-router-dom'

const UserBlocked: React.FC = () => {
  return (
    <>
        <Helmet>
            <title>User Blocked</title>
        </Helmet>
        <div className="userBlockedContainer">
            <div className="userBlockedContent">
                <h1>BLOCKED</h1>
                <p>You have been banned by one of our administrators!</p>
                <span>You can no loger log into your account!</span>
                <p className="userBlockedReturnToLogin">
                    {" " }
                    <Link to="/login" className="userLoginLink">
                      {" Return to login"}
                    </Link>
                  </p>
            </div>
        </div>
    </>
  )
}

export default UserBlocked