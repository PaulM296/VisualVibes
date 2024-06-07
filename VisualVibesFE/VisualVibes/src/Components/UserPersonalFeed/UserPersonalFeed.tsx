// src/components/MyUserProfile.tsx
import React, { useEffect, useState } from 'react';
import { Avatar, Typography } from '@mui/material';
import { MoreVert } from '@mui/icons-material';
import './UserPersonalFeed.css';
import { User } from '../../Models/User';
import { getUserIdFromToken } from '../../Utils/auth';
import { getUserById, getImageById } from '../../Services/UserServiceApi';

const MyUserProfile: React.FC = () => {
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState(true);
  const [profilePicture, setProfilePicture] = useState<string>('defaultProfilePicture.jpg');

  useEffect(() => {
    const fetchUserData = async () => {
      try {
        const token = localStorage.getItem('token');
        if (token) {
          const userId = getUserIdFromToken();
          if (userId) {
            const userData = await getUserById(userId, token);
            setUser(userData);
            if (userData.imageId) {
              try {
                const imageSrc = await getImageById(userData.imageId, token);
                setProfilePicture(imageSrc);
              } catch (error) {
                console.error('Failed to fetch image:', error);
              }
            }
          } else {
            console.error('User ID not found in token');
          }
        } else {
          console.error('Token not found in localStorage');
        }
      } catch (error) {
        console.error('Error fetching user data:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchUserData();
  }, []);

  if (loading) {
    return <p>Loading...</p>;
  }

  if (!user) {
    return <p>User not found</p>;
  }

  return (
    <div className="userPersonalFeedContainer">
      <div className="userInfo">
        <Avatar alt={user.userName} src={profilePicture} className="avatar" style={{ width: '150px', height: '150px' }} />
        <Typography className="username" style={{ marginLeft: '20px', fontSize: '24px', fontWeight: 'bold' }}>{user.userName}</Typography>
      </div>
      <Typography className="bio" style={{ marginTop: '10px', fontSize: '16px', textAlign: 'center', width: '100%' }}>
        {user.bio}
      </Typography>
      <div className="feedPost">
        <div className="feedPostWrapper">
          <div className="feedPostTop">
            <div className="feedPostTopLeft">
              <Avatar alt={user.userName} src={profilePicture} className="feedPostProfileImg" />
              <span className="feedPostUsername"> {user.userName}</span>
              <span className="feedPostDate">5 mins ago</span>
            </div>
            <div className="feedPostTopRight">
              <MoreVert />
            </div>
          </div>
          <div className="feedPostCenter">
            <span className="feedPostText"> Hey! This is my first post!</span>
            <img className="feedPostImg" src="src/assets/forestTestImage.png" alt="" />
          </div>
          <div className="feedPostBottom">
            <div className="feedPostBottomLeft">
              <img className="reactionIcon" src="src/assets/Like.png" />
              <img className="reactionIcon" src="src/assets/Love.png" />
              <span className="feedPostReactionCounter">15 people like this</span>
            </div>
            <div className="feedPostBottomRight">
              <span className="feedPostCommentText"> 7 comments</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default MyUserProfile;
