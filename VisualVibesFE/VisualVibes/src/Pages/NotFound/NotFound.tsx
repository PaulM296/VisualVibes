import React from 'react'
import { Helmet } from 'react-helmet-async'
import "./NotFound.css"

const NotFound: React.FC = () => {
  return (
    <>
        <Helmet>
            <title>404 Not Found</title>
        </Helmet>
        <div className="notFoundContainer">
            <div className="notFoundContent">
                <h1>404</h1>
                <p>Not Found</p>
                <span>The resource requested could not be found on this server!</span>
            </div>
        </div>
    </>
  )
}

export default NotFound