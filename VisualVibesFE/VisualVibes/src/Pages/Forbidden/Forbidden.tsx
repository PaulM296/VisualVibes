import React from 'react'
import './Forbidden.css'
import { Helmet } from 'react-helmet-async'

const Forbidden:React.FC = () => {
    return (
        <>
            <Helmet>
                <title>403 Forbidden</title>
            </Helmet>
            <div className="forbiddenContainer">
                <div className="forbiddenContent">
                    <h1>403</h1>
                    <p>Forbidden</p>
                    <span>You do not have permission to access the requested resource!</span>
                </div>
            </div>
        </>
      )
}

export default Forbidden