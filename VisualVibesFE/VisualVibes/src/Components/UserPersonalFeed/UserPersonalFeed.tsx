import React, { useEffect, useState } from 'react';
import { Avatar, Typography } from '@mui/material';
import { MoreVert } from '@mui/icons-material';
import './UserPersonalFeed.css';
import { User } from '../../Models/User';
import { getUserIdFromToken } from '../../Utils/auth';
import { getUserById, getImageById as getUserImageById } from '../../Services/UserServiceApi';
import { getPostsByUserId, getImageById as getPostImageById } from '../../Services/UserPostServiceApi';
import { ResponsePostModel } from '../../Models/ReponsePostModel';

const MyUserProfile: React.FC = () => {
  const [user, setUser] = useState<User | null>(null);
  const [posts, setPosts] = useState<ResponsePostModel[]>([]);
  const [postImages, setPostImages] = useState<{ [key: string]: string }>({});
  const [loading, setLoading] = useState(true);
  const [profilePicture, setProfilePicture] = useState<string>('defaultProfilePicture.jpg');

  useEffect(() => {
    const fetchUserData = async () => {
      try {
        const token = localStorage.getItem('token');
        if (!token) {
          console.error('Token not found in localStorage');
          setLoading(false);
          return;
        }

        const userId = getUserIdFromToken();
        if (!userId) {
          console.error('User ID not found in token');
          setLoading(false);
          return;
        }

        const userData = await getUserById(userId, token);
        setUser(userData);

        if (userData.imageId) {
          const imageSrc = await getUserImageById(userData.imageId, token);
          setProfilePicture(imageSrc);
        }

        const userPosts = await getPostsByUserId(userId, token);
        setPosts(userPosts);

        const imagesPromises = userPosts.map(async (post) => {
          if (post.imageId) {
            try {
              const imageSrc = await getPostImageById(post.imageId, token);
              return { postId: post.id, imageSrc };
            } catch (error) {
              console.error(`Failed to fetch image for post ${post.id}:`, error);
              return { postId: post.id, imageSrc: '' };
            }
          }
          return { postId: post.id, imageSrc: '' };
        });

        const images = await Promise.all(imagesPromises);
        const imagesMap = images.reduce((acc, { postId, imageSrc }) => {
          if (imageSrc) {
            acc[postId] = imageSrc;
          }
          return acc;
        }, {} as { [key: string]: string });

        setPostImages(imagesMap);

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
      {posts.map((post) => (
        <div key={post.id} className="feedPost">
          <div className="feedPostWrapper">
            <div className="feedPostTop">
              <div className="feedPostTopLeft">
                <Avatar alt={user.userName} src={profilePicture} className="feedPostProfileImg" />
                <span className="feedPostUsername"> {user.userName}</span>
                <span className="feedPostDate">{new Date(post.createdAt).toLocaleTimeString()}</span>
              </div>
              <div className="feedPostTopRight">
                <MoreVert />
              </div>
            </div>
            <div className="feedPostCenter">
              <span className="feedPostText"> {post.caption}</span>
              {post.imageId && postImages[post.id] && (
                <img className="feedPostImg" src={postImages[post.id]} alt="Post image" />
              )}
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
      ))}
    </div>
  );
};

export default MyUserProfile;
