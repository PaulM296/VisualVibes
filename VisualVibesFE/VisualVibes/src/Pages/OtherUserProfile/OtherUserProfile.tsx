import React from 'react'
import './OtherUserProfile.css';
import { Helmet } from 'react-helmet-async';
import Sidebar from '../../Components/Sidebar/Sidebar';
import RightBarUserProfile from '../../Components/RightBarUserProfile/RightBarUserProfile';
import Navbar from '../../Components/Navbar/Navbar';
import OtherUsersProfile from '../../Components/OtherUsersProfile/OtherUsersProfile';

const OtherUserProfile: React.FC = () => {
  return (
    <>
      <Helmet>
        <title>Profile</title>
      </Helmet>
      <div className="otherUserProfileContainer">
        <Navbar />
        <div className="otherUserProfileContent">
          <Sidebar />
          <OtherUsersProfile />
          <RightBarUserProfile />
        </div>
      </div>
    </>
  );
}

export default OtherUserProfile