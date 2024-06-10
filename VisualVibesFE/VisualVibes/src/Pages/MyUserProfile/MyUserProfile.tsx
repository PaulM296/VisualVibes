import React from 'react'
import { Helmet } from 'react-helmet-async'
import Navbar from '../../Components/Navbar/Navbar'
import Sidebar from '../../Components/Sidebar/Sidebar'
//import UserPersonalFeed from '../../Components/UserPersonalFeed/UserPersonalFeed'
import './MyUserProfile.css'
import RightBarUserProfile from '../../Components/RightBarUserProfile/RightBarUserProfile'
import UserPersonalFeedTest from '../../Components/UserPersonalFeedTest/UserPersonalFeedTest'

const MyUserProfile: React.FC = () => {
  return (
    <>
        <Helmet>
          <title>MyProfile</title>
        </Helmet>
        <div className="userProfileContainer">
          <Navbar />
        <div className="userProfileContent">
          <Sidebar />
          {/* <UserPersonalFeed /> */}
          <UserPersonalFeedTest />
          <RightBarUserProfile />
        </div>
      </div>
    </>
    
  )
}

export default MyUserProfile;