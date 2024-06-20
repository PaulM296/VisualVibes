import React from 'react'
import { Helmet } from 'react-helmet-async'
import Navbar from '../../Components/Navbar/Navbar'
import Sidebar from '../../Components/Sidebar/Sidebar'
import './MyUserProfile.css'
import UserPersonalFeedTest from '../../Components/UserPersonalFeedTest/UserPersonalFeedTest'

const MyUserProfile: React.FC = () => {
  return (
    <div className="userProfile">
        <Helmet>
          <title>MyProfile</title>
        </Helmet>
        <div className="userProfileContainer">
          <Navbar />
        <div className="userProfileContent">
          <div className="myProfileStickySidebar">
            <Sidebar />
          </div>
          <UserPersonalFeedTest />
        </div>
      </div>
    </div> 
  )
}

export default MyUserProfile;