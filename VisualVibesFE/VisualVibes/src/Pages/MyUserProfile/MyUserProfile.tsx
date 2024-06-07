import React from 'react'
import { Helmet } from 'react-helmet-async'
import Navbar from '../../Components/Navbar/Navbar'
import Sidebar from '../../Components/Sidebar/Sidebar'
import UserPersonalFeed from '../../Components/UserPersonalFeed/UserPersonalFeed'
import './MyUserProfile.css'
import RightBarUserProfile from '../../Components/RightBarUserProfile/RightBarUserProfile'

const MyUserProfile: React.FC = () => {
  return (
    <>
        <Helmet>
            MyProfile
        </Helmet>
        <div className="userProfileContainer">
          <Navbar />
        <div className="userProfileContent">
          <Sidebar />
          <UserPersonalFeed />
          <RightBarUserProfile />
        </div>
      </div>
    </>
    
  )
}

export default MyUserProfile;