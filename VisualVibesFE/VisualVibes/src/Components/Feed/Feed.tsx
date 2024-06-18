import React, { useEffect, useState } from 'react';
import { Avatar, Typography, Modal, Button, TextField } from '@mui/material';
import { getUserFeed } from '../../Services/UserFeedServiceApi';
import { getImageById as getUserImageById } from '../../Services/UserServiceApi';
import { getImageById as getPostImageById } from '../../Services/UserPostServiceApi';
import { addReaction, deleteReaction, getPostReactions, updateReaction } from '../../Services/ReactionServiceApi';
import { getPostComments, addComment, updateComment, deleteComment } from '../../Services/CommentServiceApi';
import { FeedPost, UserFeed } from '../../Models/FeedPostInterface';
import { getReactionEmoji } from '../../Utils/getReactionEmoji';
import { ResponseReaction } from '../../Models/ResponseReaction';
import { ReactionType } from '../../Models/ReactionType';
import { ReactionWithEmoji } from '../../Models/ReactionWithEmoji';
import { ResponseComment, FormattedComment } from '../../Models/ResponseComment';
import RichTextEditor from '../RichTextEditor/RichTextEditor';
import { getUserIdFromToken } from '../../Utils/auth';
import { PaginationResponse } from '../../Models/PaginationResponse';
import './Feed.css';
import { reactionTypes } from '../../Utils/const/reactionTypes';
import { formatPostDate } from '../../Utils/formatPostDateUtil';

const Feed: React.FC = () => {
  const [posts, setPosts] = useState<FeedPost[]>([]);
  const [postImages, setPostImages] = useState<{ [key: string]: string }>({});
  const [profileImages, setProfileImages] = useState<{ [key: string]: string }>({});
  const [comments, setComments] = useState<FormattedComment[]>([]);
  const [newComment, setNewComment] = useState<string>('');
  const [loading, setLoading] = useState(true);
  const [showReactions, setShowReactions] = useState<{ [key: string]: boolean }>({});
  const [openReactionModal, setOpenReactionModal] = useState(false);
  const [openCommentModal, setOpenCommentModal] = useState(false);
  const [reactions, setReactions] = useState<ReactionWithEmoji[]>([]);
  const [userReactions, setUserReactions] = useState<{ [key: string]: string }>({});
  const [pageIndex, setPageIndex] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [reactionsCount, setReactionsCount] = useState<{ [key: string]: number }>({});
  const [commentsCount, setCommentsCount] = useState<{ [key: string]: number }>({});
  const [currentPostId, setCurrentPostId] = useState<string | null>(null);
  const [currentCommentPageIndex, setCurrentCommentPageIndex] = useState(1);
  const [commentTotalPages, setCommentTotalPages] = useState(1);
  const [editingCommentId, setEditingCommentId] = useState<string | null>(null);
  const [editCommentText, setEditCommentText] = useState<string>('');

  useEffect(() => {
    fetchFeedData(1);
  }, []);

  const fetchFeedData = async (pageIndex: number) => {
    try {
      const data: PaginationResponse<UserFeed> = await getUserFeed(pageIndex, 10);
      const newPosts = data.items[0].posts;
  
      setPosts((prevPosts) => (pageIndex === 1 ? newPosts : [...prevPosts, ...newPosts]));
      setPageIndex(data.pageIndex);
      setTotalPages(data.totalPages);
  
      const imagesPromises = newPosts.map(async (post) => {
        const postImagePromise = post.postImageId
          ? getPostImageById(post.postImageId)
          : Promise.resolve('');
        const profileImagePromise = post.userProfileImageId
          ? getUserImageById(post.userProfileImageId)
          : Promise.resolve('');
  
        const [postImageSrc, profileImageSrc] = await Promise.all([postImagePromise, profileImagePromise]);
  
        return {
          postId: post.postId,
          postImageSrc,
          userProfileImageId: post.userProfileImageId,
          profileImageSrc,
        };
      });
  
      const images = await Promise.all(imagesPromises);
  
      const postImagesMap = images.reduce((acc, { postId, postImageSrc }) => {
        if (postImageSrc) {
          acc[postId] = postImageSrc;
        }
        return acc;
      }, {} as { [key: string]: string });
  
      const profileImagesMap = images.reduce((acc, { userProfileImageId, profileImageSrc }) => {
        if (userProfileImageId && profileImageSrc) {
          acc[userProfileImageId] = profileImageSrc;
        }
        return acc;
      }, {} as { [key: string]: string });
  
      setPostImages((prevImages) => ({ ...prevImages, ...postImagesMap }));
      setProfileImages((prevImages) => ({ ...prevImages, ...profileImagesMap }));
  
      const reactionsMap = newPosts.reduce((acc, post) => {
        acc[post.postId] = post.reactions.length;
        return acc;
      }, {} as { [key: string]: number });
  
      const commentsMap = newPosts.reduce((acc, post) => {
        acc[post.postId] = post.comments.length;
        return acc;
      }, {} as { [key: string]: number });
  
      const userReactionsMap = newPosts.reduce((acc, post) => {
        const userReaction = post.reactions.find((r) => r.userId === getUserIdFromToken());
        if (userReaction) {
          acc[post.postId] = ReactionType[userReaction.reactionType];
        }
        return acc;
      }, {} as { [key: string]: string });
  
      setReactionsCount((prev) => ({ ...prev, ...reactionsMap }));
      setCommentsCount((prev) => ({ ...prev, ...commentsMap }));
      setUserReactions((prev) => ({ ...prev, ...userReactionsMap }));
    } catch (error) {
      console.error('Error fetching feed data:', error);
    } finally {
      setLoading(false);
    }
  };  

 
  const handleReaction = async (postId: string, reactionType: string) => {
    try {
      const currentUserReactionType = userReactions[postId];
      const reactionTypeId = reactionTypes[reactionType];
      const postIndex = posts.findIndex(post => post.postId === postId);
      const reaction = posts[postIndex]?.reactions.find(r => r.userId === getUserIdFromToken());

      if (currentUserReactionType) {
        if (currentUserReactionType === reactionType) {
          if (reaction) {
            await deleteReaction(reaction.id);
            setReactionsCount(prev => ({ ...prev, [postId]: prev[postId] - 1 }));
            setUserReactions(prev => {
              const newUserReactions = { ...prev };
              delete newUserReactions[postId];
              return newUserReactions;
            });
            setPosts(prevPosts => {
              const updatedPosts = [...prevPosts];
              updatedPosts[postIndex].reactions = updatedPosts[postIndex].reactions.filter(r => r.id !== reaction.id);
              return updatedPosts;
            });
          }
        } else {
          if (reaction) {
            await updateReaction(reaction.id, reactionTypeId);
            setUserReactions(prev => ({ ...prev, [postId]: reactionType }));
            setPosts(prevPosts => {
              const updatedPosts = [...prevPosts];
              const reactionIndex = updatedPosts[postIndex].reactions.findIndex(r => r.id === reaction.id);
              updatedPosts[postIndex].reactions[reactionIndex].reactionType = reactionTypeId;
              return updatedPosts;
            });
          }
        }
      } else {
        const newReaction = await addReaction(postId, reactionTypeId);
        setReactionsCount(prev => ({ ...prev, [postId]: (prev[postId] || 0) + 1 }));
        setUserReactions(prev => ({ ...prev, [postId]: reactionType }));
        setPosts(prevPosts => {
          const updatedPosts = [...prevPosts];
          updatedPosts[postIndex].reactions.push(newReaction);
          return updatedPosts;
        });
      }
    } catch (error) {
      console.error('Error handling reaction:', error);
    }
  };


  const fetchReactions = async (postId: string, pageIndex: number = 1) => {
    try {
      const reactionData = await getPostReactions(postId, pageIndex);
      const formattedReactions: ReactionWithEmoji[] = await Promise.all(
        reactionData.items.map(async (reaction: ResponseReaction) => {
          const avatar = reaction.imageId ? await getUserImageById(reaction.imageId) : '';
          return {
            userName: reaction.userName,
            avatar,
            reactionType: ReactionType[reaction.reactionType],
            reactionEmoji: getReactionEmoji(ReactionType[reaction.reactionType]),
          };
        })
      );
      setReactions(formattedReactions);
      setCurrentPostId(postId);
      setPageIndex(pageIndex);
      setTotalPages(reactionData.totalPages);
      setOpenReactionModal(true);
    } catch (error) {
      console.error('Error fetching reactions:', error);
    }
  };

  const fetchComments = async (postId: string, pageIndex: number = 1, pageSize: number = 10) => {
    try {
      const commentData = await getPostComments(postId, pageIndex, pageSize);

      if (!commentData.items || commentData.items.length === 0) {
        setComments([]);
        setCommentTotalPages(1);
      } else {
        const formattedComments: FormattedComment[] = await Promise.all(
          commentData.items.map(async (comment: ResponseComment) => {
            const avatar = comment.imageId ? await getUserImageById(comment.imageId) : '';
            return {
              id: comment.id,
              userId: comment.userId,
              userName: comment.userName,
              avatar,
              text: comment.text,
              createdAt: comment.createdAt,
            };
          })
        );
        setComments(formattedComments);
        setCommentTotalPages(commentData.totalPages);
      }
      setCurrentPostId(postId);
      setCurrentCommentPageIndex(pageIndex);
      setOpenCommentModal(true);
    } catch (error) {
      console.error('Error fetching comments:', error);
      setComments([]);
      setCommentTotalPages(1);
      setOpenCommentModal(true);
    }
  };

  const handleAddComment = async () => {
    try {
      const token = localStorage.getItem('token');
      if (!token || !currentPostId || !newComment) return;

      await addComment(currentPostId, newComment);

      setCommentsCount((prev) => ({
        ...prev,
        [currentPostId]: (prev[currentPostId] || 0) + 1,
      }));

      setNewComment('');
      fetchComments(currentPostId, currentCommentPageIndex);
    } catch (error) {
      console.error('Error adding comment:', error);
    }
  };

  const handleEditComment = (comment: FormattedComment) => {
    setEditingCommentId(comment.id);
    setEditCommentText(comment.text);
  };

  const handleUpdateComment = async () => {
    try {
      const token = localStorage.getItem('token');
      if (!token || !editingCommentId || !editCommentText) return;

      await updateComment(editingCommentId, editCommentText);
      setEditingCommentId(null);
      setEditCommentText('');
      fetchComments(currentPostId!, currentCommentPageIndex);
    } catch (error) {
      console.error('Error updating comment:', error);
    }
  };

  const handleDeleteComment = async (commentId: string) => {
    try {
      await deleteComment(commentId);
      fetchComments(currentPostId!, currentCommentPageIndex);
    } catch (error) {
      console.error('Error deleting comment:', error);
    }
  };

  const handleOpenComments = (postId: string) => {
    setCurrentPostId(postId);
    setComments([]);
    fetchComments(postId);
  };

  const handleClose = () => {
    setOpenReactionModal(false);
    setOpenCommentModal(false);
    setCurrentPostId(null);
  };

  const loadMorePosts = async () => {
    try {
      const token = localStorage.getItem('token');
      if (!token) return;
      await fetchFeedData(pageIndex + 1);
    } catch (error) {
      console.error('Error loading more posts:', error);
    }
  };

  if (loading) {
    return <p>Loading...</p>;
  }

  if (!posts.length) {
    return <p>No feed data available</p>;
  }

  return (
    <div className="feed">
      <div className="feedWrapper">
        {posts.map((post) => (
          <div key={post.postId} className="feedPost">
            <div className="feedPostWrapper">
              <div className="feedPostTop">
                <div className="feedPostTopLeft">
                  <Avatar
                    alt={post.userName}
                    src={profileImages[post.userProfileImageId] || 'defaultProfilePicture.jpg'}
                    className="feedPostProfileImg"
                  />
                  <span className="feedPostUsername">{post.userName}</span>
                </div>
                <div className="feedPostTopRight">
                  <span className="feedPostDate">{formatPostDate(post.createdAt)}</span>
                </div>
              </div>
              <div className="feedPostCenter">
                <span className="feedPostText" dangerouslySetInnerHTML={{ __html: post.caption || '' }}></span>
                {post.postImageId && postImages[post.postId] && (
                  <img className="feedPostImg" src={postImages[post.postId]} alt="Post image" />
                )}
              </div>
              <div className="feedPostBottom">
                <div className="feedPostBottomLeft">
                  <div className="reactionButton"
                    onMouseEnter={() => setShowReactions(prev => ({ ...prev, [post.postId]: true }))}
                    onMouseLeave={() => setShowReactions(prev => ({ ...prev, [post.postId]: false }))}>
                    {userReactions[post.postId] ? (
                      <span className="reactionOpener selected" role="img" aria-label={userReactions[post.postId]}>
                        {getReactionEmoji(userReactions[post.postId])}
                      </span>
                    ) : (
                      <span className="reactionOpener" role="img" aria-label="thumbs up">👍</span>
                    )}
                    {showReactions[post.postId] && (
                      <div className="reactionOptions">
                        {Object.keys(reactionTypes).map(type => (
                          <span key={type}
                                className={userReactions[post.postId] === type ? 'selected' : ''}
                                role="img"
                                aria-label={type}
                                onClick={() => handleReaction(post.postId, type)}>
                            {getReactionEmoji(type)}
                          </span>
                        ))}
                      </div>
                    )}
                  <span className="feedPostReactionCounter" onClick={() => fetchReactions(post.postId)}>
                    {reactionsCount[post.postId] || 0} people reacted
                  </span>
                </div>
                </div>
                <div className="feedPostBottomRight">
                  <span 
                    className="feedPostCommentText" 
                    onClick={() => handleOpenComments(post.postId)}
                    onMouseEnter={(e) => (e.currentTarget.style.color = 'blue')}
                    onMouseLeave={(e) => (e.currentTarget.style.color = 'black')}
                    style={{ cursor: 'pointer' }}
                  >
                    {commentsCount[post.postId] || 0} comments
                  </span>
                </div>
              </div>
            </div>
          </div>
        ))}
        {pageIndex < totalPages && (
          <div className="centerButton">
            <Button onClick={loadMorePosts} variant="contained" color="primary" style={{ margin: 'auto', display: 'block' }}>
              Load More
            </Button>
          </div>
        )}
      </div>
      <Modal open={openReactionModal} onClose={handleClose}>
        <div className="modalContent">
          <Typography variant="h6">Reactions</Typography>
          {reactions.length === 0 && <Typography sx={{ mt: 2 }}>No reactions yet.</Typography>}
          {reactions.length > 0 &&
            reactions.map((reaction, index) => (
              <div key={index} className="reactionItem">
                <span>{reaction.reactionEmoji}</span>
                <Avatar src={reaction.avatar} alt={reaction.userName} sx={{ margin: '0 10px' }} />
                <Typography>{reaction.userName}</Typography>
              </div>
            ))}
          <div className="paginationControls">
            <Button
              disabled={pageIndex === 1}
              onClick={() => fetchReactions(currentPostId!, pageIndex - 1)}
              style={{ float: 'left' }}
            >
              Previous
            </Button>
            <Typography>{pageIndex} / {totalPages}</Typography>
            <Button
              disabled={pageIndex === totalPages}
              onClick={() => fetchReactions(currentPostId!, pageIndex + 1)}
              style={{ float: 'right' }}
            >
              Next
            </Button>
          </div>
        </div>
      </Modal>
      <Modal open={openCommentModal} onClose={handleClose}>
        <div className="modalContentLarge">
          <Typography variant="h6">Comments</Typography>
          <div className="addCommentContainer">
            <RichTextEditor content={newComment} setContent={setNewComment} />
            <Button variant="contained" color="primary" onClick={handleAddComment} style={{ marginTop: '10px' }}>
              Submit
            </Button>
          </div>
          <div className="commentsContainer">
            {comments.length === 0 && <Typography sx={{ mt: 2 }}>No comments yet.</Typography>}
            {comments.length > 0 &&
              comments.map((comment, index) => (
                <div key={index} className="commentItem">
                  <Avatar src={comment.avatar} alt={comment.userName} sx={{ margin: '0 10px' }} />
                  <div>
                    <Typography>{comment.userName}</Typography>
                    {editingCommentId === comment.id ? (
                      <>
                        <TextField
                          value={editCommentText}
                          onChange={(e) => setEditCommentText(e.target.value)}
                          fullWidth
                          multiline
                        />
                        <Button variant="contained" color="primary" onClick={handleUpdateComment}>
                          Save
                        </Button>
                        <Button onClick={() => setEditingCommentId(null)}>Cancel</Button>
                      </>
                    ) : (
                      <Typography sx={{ marginLeft: '10px' }} dangerouslySetInnerHTML={{ __html: comment.text }}></Typography>
                    )}
                  </div>
                  {comment.userId === getUserIdFromToken() && (
                    <div>
                      <Button onClick={() => handleEditComment(comment)}>Edit</Button>
                      <Button onClick={() => handleDeleteComment(comment.id)}>Delete</Button>
                    </div>
                  )}
                </div>
              ))}
            <div className="paginationControls">
              <Button
                disabled={currentCommentPageIndex === 1}
                onClick={() => fetchComments(currentPostId!, currentCommentPageIndex - 1)}
                style={{ float: 'left' }}
              >
                Previous
              </Button>
              <Typography>{currentCommentPageIndex} / {commentTotalPages}</Typography>
              <Button
                disabled={currentCommentPageIndex === commentTotalPages}
                onClick={() => fetchComments(currentPostId!, currentCommentPageIndex + 1)}
                style={{ float: 'right' }}
              >
                Next
              </Button>
            </div>
          </div>
        </div>
      </Modal>
    </div>
  );
};

export default Feed;
