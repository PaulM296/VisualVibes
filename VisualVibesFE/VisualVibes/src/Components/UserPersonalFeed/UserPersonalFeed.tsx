import React, { useEffect, useState } from 'react';
import { Avatar, Typography } from '@mui/material';
import { differenceInDays, differenceInHours, differenceInMinutes } from 'date-fns';
import './UserPersonalFeed.css';
import { User } from '../../Models/User';
import { getUserIdFromToken } from '../../Utils/auth';
import { getUserById, getImageById as getUserImageById } from '../../Services/UserServiceApi';
import { getPostsByUserId, getImageById as getPostImageById } from '../../Services/UserPostServiceApi';
import { getReactionsCountByPostId } from '../../Services/ReactionServiceApi';
import { getCommentsCountByPostId } from '../../Services/CommentServiceApi';
import { ResponsePostModel } from '../../Models/ReponsePostModel';

const MyUserProfile: React.FC = () => {
  const [user, setUser] = useState<User | null>(null);
  const [posts, setPosts] = useState<ResponsePostModel[]>([]);
  const [postImages, setPostImages] = useState<{ [key: string]: string }>({});
  const [reactionsCount, setReactionsCount] = useState<{ [key: string]: number }>({});
  const [commentsCount, setCommentsCount] = useState<{ [key: string]: number }>({});
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

        const reactionsPromises = userPosts.map(async (post) => {
          try {
            const count = await getReactionsCountByPostId(post.id, token);
            return { postId: post.id, count };
          } catch (error) {
            console.error(`Failed to fetch reactions count for post ${post.id}:`, error);
            return { postId: post.id, count: 0 };
          }
        });

        const commentsPromises = userPosts.map(async (post) => {
          try {
            const count = await getCommentsCountByPostId(post.id, token);
            return { postId: post.id, count };
          } catch (error) {
            console.error(`Failed to fetch comments count for post ${post.id}:`, error);
            return { postId: post.id, count: 0 };
          }
        });

        const reactionsCounts = await Promise.all(reactionsPromises);
        const commentsCounts = await Promise.all(commentsPromises);

        const reactionsMap = reactionsCounts.reduce((acc, { postId, count }) => {
          acc[postId] = count;
          return acc;
        }, {} as { [key: string]: number });

        const commentsMap = commentsCounts.reduce((acc, { postId, count }) => {
          acc[postId] = count;
          return acc;
        }, {} as { [key: string]: number });

        setReactionsCount(reactionsMap);
        setCommentsCount(commentsMap);

      } catch (error) {
        console.error('Error fetching user data:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchUserData();
  }, []);

  const formatPostDate = (date: Date | string) => {
    if (typeof date === 'string') {
      date = new Date(date);
    }
    const now = new Date();

    const adjustedDate = new Date(date.getTime() + 3 * 60 * 60 * 1000);

    const minutesDifference = differenceInMinutes(now, adjustedDate);
    const hoursDifference = differenceInHours(now, adjustedDate);
    const daysDifference = differenceInDays(now, adjustedDate);

    if (minutesDifference < 60) {
      return `Created ${minutesDifference} minutes ago`;
    } else if (hoursDifference < 24) {
      return `Created ${hoursDifference} hours ago`;
    } else {
      return `Created ${daysDifference} days ago`;
    }
  };

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
              </div>
              <div className="feedPostTopRight">
                <span className="feedPostDate">{formatPostDate(post.createdAt)}</span>
              </div>
            </div>
            <div className="feedPostCenter">
              <span className="feedPostText" dangerouslySetInnerHTML={{ __html: post.caption }}></span>
              {post.imageId && postImages[post.id] && (
                <img className="feedPostImg" src={postImages[post.id]} alt="Post image" />
              )}
            </div>
            <div className="feedPostBottom">
              <div className="feedPostBottomLeft">
                <img className="reactionIcon" src="src/assets/Like.png" />
                <img className="reactionIcon" src="src/assets/Love.png" />
                <span className="feedPostReactionCounter">{reactionsCount[post.id] || 0} people reacted</span>
              </div>
              <div className="feedPostBottomRight">
                <span className="feedPostCommentText">{commentsCount[post.id] || 0} comments</span>
              </div>
            </div>
          </div>
        </div>
      ))}
    </div>
  );
};

export default MyUserProfile;
