import React, { useEffect, useState } from 'react';
import { Avatar, Typography, Modal } from '@mui/material';
import { differenceInDays, differenceInHours, differenceInMinutes } from 'date-fns';
import './UserPersonalFeed.css';
import { User } from '../../Models/User';
import { getUserIdFromToken } from '../../Utils/auth';
import { getUserById, getImageById as getUserImageById } from '../../Services/UserServiceApi';
import { getPostsByUserId, getImageById as getPostImageById } from '../../Services/UserPostServiceApi';
import { addReaction, getReactionsCountByPostId, getPostReactions, getUserReactionByPostId } from '../../Services/ReactionServiceApi';
import { getCommentsCountByPostId, getPostComments } from '../../Services/CommentServiceApi';
import { ResponsePostModel } from '../../Models/ReponsePostModel';
import { getReactionEmoji } from '../../Utils/getReactionEmoji';
import { ResponseReaction } from '../../Models/ResponseReaction';
import { ReactionType } from '../../Models/ReactionType';
import { ReactionWithEmoji } from '../../Models/ReactionWithEmoji';

const MyUserProfile: React.FC = () => {
  const [user, setUser] = useState<User | null>(null);
  const [posts, setPosts] = useState<ResponsePostModel[]>([]);
  const [postImages, setPostImages] = useState<{ [key: string]: string }>({});
  const [reactionsCount, setReactionsCount] = useState<{ [key: string]: number }>({});
  const [commentsCount, setCommentsCount] = useState<{ [key: string]: number }>({});
  const [comments, setComments] = useState<{ userName: string; avatar: string; commentText: string }[]>([]);
  const [loading, setLoading] = useState(true);
  const [profilePicture, setProfilePicture] = useState<string>('defaultProfilePicture.jpg');
  const [showReactions, setShowReactions] = useState<{ [key: string]: boolean }>({});
  const [openReactionModal, setOpenReactionModal] = useState(false);
  const [openCommentModal, setOpenCommentModal] = useState(false);
  const [reactions, setReactions] = useState<ReactionWithEmoji[]>([]);
  const [userReactions, setUserReactions] = useState<{ [key: string]: string }>({});

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
  
        const userReactionsPromises = userPosts.map(async (post) => {
          try {
            const reaction = await getUserReactionByPostId(post.id, userId, token);
            return { postId: post.id, reaction };
          } catch (error) {
            console.error(`Failed to fetch user reaction for post ${post.id}:`, error);
            return { postId: post.id, reaction: null };
          }
        });
  
        const reactionsCounts = await Promise.all(reactionsPromises);
        const commentsCounts = await Promise.all(commentsPromises);
        const userReactions = await Promise.all(userReactionsPromises);
  
        const reactionsMap = reactionsCounts.reduce((acc, { postId, count }) => {
          acc[postId] = count;
          return acc;
        }, {} as { [key: string]: number });
  
        const commentsMap = commentsCounts.reduce((acc, { postId, count }) => {
          acc[postId] = count;
          return acc;
        }, {} as { [key: string]: number });
  
        const userReactionsMap = userReactions.reduce((acc, { postId, reaction }) => {
          if (reaction !== null) {
            acc[postId] = ReactionType[reaction];
          }
          return acc;
        }, {} as { [key: string]: string });
  
        setReactionsCount(reactionsMap);
        setCommentsCount(commentsMap);
        setUserReactions(userReactionsMap);
  
      } catch (error) {
        console.error('Error fetching user data:', error);
      } finally {
        setLoading(false);
      }
    };
  
    fetchUserData();
  }, []);
  
  const reactionTypes: { [key: string]: number } = {
    'Like': ReactionType.Like,
    'Love': ReactionType.Love,
    'Laugh': ReactionType.Laugh,
    'Cry': ReactionType.Cry,
    'Anger': ReactionType.Anger
  };

  const handleReaction = async (postId: string, reactionType: string) => {
    try {
      const token = localStorage.getItem('token');
      if (!token) {
        console.error('Token not found in localStorage');
        return;
      }

      const reactionTypeId = reactionTypes[reactionType];
      if (reactionTypeId === undefined) {
        console.error('Invalid reaction type:', reactionType);
        return;
      }

      await addReaction(postId, reactionTypeId, token);
      const count = await getReactionsCountByPostId(postId, token);
      setReactionsCount((prev) => ({ ...prev, [postId]: count }));
    } catch (error) {
      console.error('Error adding reaction:', error);
    }
  };

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

  const fetchReactions = async (postId: string) => {
    try {
      const token = localStorage.getItem('token');
      if (!token) {
        console.error('Token not found in localStorage');
        return;
      }
  
      const reactionData = await getPostReactions(postId, token);
      const formattedReactions: ReactionWithEmoji[] = await Promise.all(reactionData.items.map(async (reaction: ResponseReaction) => {
        const avatar = reaction.imageId ? await getUserImageById(reaction.imageId, token) : '';
        return {
          userName: reaction.userName,  
          avatar,  
          reactionType: ReactionType[reaction.reactionType],
          reactionEmoji: getReactionEmoji(ReactionType[reaction.reactionType])
        };
      }));
      setReactions(formattedReactions);
      setOpenReactionModal(true);
    } catch (error) {
      console.error('Error fetching reactions:', error);
    }
  };

  const fetchComments = async (postId: string) => {
    try {
      const token = localStorage.getItem('token');
      if (!token) {
        console.error('Token not found in localStorage');
        return;
      }
  
      console.log('Fetching comments for post ID:', postId);
  
      const commentData = await getPostComments(postId, token);
      console.log('Fetched comments:', commentData);
  
      setComments(commentData.items);
      setOpenCommentModal(true);
    } catch (error) {
      console.error('Error fetching comments:', error);
    }
  };

  const handleClose = () => {
    setOpenReactionModal(false);
    setOpenCommentModal(false);
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
                <div
                  className="reactionButton"
                  onMouseEnter={() => setShowReactions((prev) => ({ ...prev, [post.id]: true }))}
                  onMouseLeave={() => setShowReactions((prev) => ({ ...prev, [post.id]: false }))}
                >
                  {userReactions[post.id] ? (
                    <span className="reactionOpener" role="img" aria-label={userReactions[post.id]}>{getReactionEmoji(userReactions[post.id])}</span>
                  ) : (
                    <span className="reactionOpener" role="img" aria-label="thumbs up">👍</span>
                  )}
                  {showReactions[post.id] && (
                    <div className="reactionOptions">
                      <span role="img" aria-label="thumbs up" onClick={() => handleReaction(post.id, 'Like')}>👍</span>
                      <span role="img" aria-label="heart" onClick={() => handleReaction(post.id, 'Love')}>❤️</span>
                      <span role="img" aria-label="laughing" onClick={() => handleReaction(post.id, 'Laugh')}>😂</span>
                      <span role="img" aria-label="crying" onClick={() => handleReaction(post.id, 'Cry')}>😢</span>
                      <span role="img" aria-label="angry" onClick={() => handleReaction(post.id, 'Anger')}>😠</span>
                    </div>
                  )}
                  <span 
                    className="feedPostReactionCounter" 
                    onClick={() => fetchReactions(post.id)}
                    onMouseEnter={(e) => (e.currentTarget.style.color = 'blue')}
                    onMouseLeave={(e) => (e.currentTarget.style.color = 'black')}
                    style={{ cursor: 'pointer' }}
                  >
                    {reactionsCount[post.id] || 0} people reacted
                  </span>
                </div>
              </div>
              <div className="feedPostBottomRight">
                <span 
                  className="feedPostCommentText" 
                  onClick={() => fetchComments(post.id)}
                  onMouseEnter={(e) => (e.currentTarget.style.color = 'blue')}
                  onMouseLeave={(e) => (e.currentTarget.style.color = 'black')}
                  style={{ cursor: 'pointer' }}
                >
                  {commentsCount[post.id] || 0} comments
                </span>
              </div>
            </div>
          </div>
        </div>
      ))}
      <Modal
        open={openReactionModal}
        onClose={handleClose}
      >
        <div className="modalContent">
          <Typography variant="h6">Reactions</Typography>
          {reactions.length === 0 && (
            <Typography sx={{ mt: 2 }}>No reactions yet.</Typography>
          )}
          {reactions.length > 0 && reactions.map((reaction, index) => (
            <div key={index} className="reactionItem">
              <span>{reaction.reactionEmoji}</span>
              <Avatar src={reaction.avatar} alt={reaction.userName} sx={{ margin: '0 10px' }} />
              <Typography>{reaction.userName}</Typography>
            </div>
          ))}
        </div>
      </Modal>
      <Modal
        open={openCommentModal}
        onClose={handleClose}
      >
        <div className="modalContent">
          <Typography variant="h6">Comments</Typography>
          {comments.length === 0 && (
            <Typography sx={{ mt: 2 }}>No comments yet.</Typography>
          )}
          {comments.length > 0 && comments.map((comment, index) => (
            <div key={index} className="reactionItem">
              <Avatar src={comment.avatar} alt={comment.userName} sx={{ margin: '0 10px' }} />
              <Typography>{comment.userName}</Typography>
              <Typography sx={{ marginLeft: '10px' }}>{comment.commentText}</Typography>
            </div>
          ))}
        </div>
      </Modal>
    </div>
  );
};

export default MyUserProfile;
