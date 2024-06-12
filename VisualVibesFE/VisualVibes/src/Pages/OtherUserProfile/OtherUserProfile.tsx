import React from 'react'
import './OtherUserProfile.css';
import { Helmet } from 'react-helmet-async';
import RightBarUserProfile from '../../Components/RightBarUserProfile/RightBarUserProfile';
import Navbar from '../../Components/Navbar/Navbar';
import OtherUsersProfile from '../../Components/OtherUsersProfile/OtherUsersProfile';
import OtherUserSidebar from '../../Components/OtherUserSidebar/OtherUserSideBar';

const OtherUserProfile: React.FC = () => {
  return (
    <>
      <Helmet>
        <title>Profile</title>
      </Helmet>
      <div className="otherUserProfileContainer">
        <Navbar />
        <div className="otherUserProfileContent">
          <OtherUserSidebar />
          <OtherUsersProfile />
          <RightBarUserProfile />
        </div>
      </div>
    </>
  );
}

export default OtherUserProfile