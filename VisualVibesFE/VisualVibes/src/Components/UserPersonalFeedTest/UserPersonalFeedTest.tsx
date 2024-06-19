import React, { useEffect, useState } from 'react';
import { Button } from '@mui/material';
import './UserPersonalFeedTest.css';
import { User } from '../../Models/User';
import { getUserIdFromToken } from '../../Utils/auth';
import { getUserById, getImageById as getUserImageById } from '../../Services/UserServiceApi';
import { getPostsByUserId, getImageById as getPostImageById, updatePost, removePost } from '../../Services/UserPostServiceApi';
import { addReaction, deleteReaction, getPostReactions, updateReaction } from '../../Services/ReactionServiceApi';
import { getPostComments, addComment, updateComment, deleteComment } from '../../Services/CommentServiceApi';
import { ResponsePostModel } from '../../Models/ReponsePostModel';
import { getReactionEmoji } from '../../Utils/getReactionEmoji';
import { ResponseReaction } from '../../Models/ResponseReaction';
import { ReactionType } from '../../Models/ReactionType';
import { ReactionWithEmoji } from '../../Models/ReactionWithEmoji';
import { PaginationRequestDto, PaginationResponse } from '../../Models/PaginationResponse';
import { ResponseComment, FormattedComment } from '../../Models/ResponseComment';
import UserProfileInfo from '../UserProfileInfo';
import Post from '../Post/Post';
import ReactionModal from '../ReactionModal';
import CommentModal from '../CommentModal';
import { reactionTypes } from '../../Utils/const/reactionTypes';

const MyUserProfile: React.FC = () => {
  const [user, setUser] = useState<User | null>(null);
  const [posts, setPosts] = useState<ResponsePostModel[]>([]);
  const [postImages, setPostImages] = useState<{ [key: string]: string }>({});
  const [comments, setComments] = useState<FormattedComment[]>([]);
  const [newComment, setNewComment] = useState<string>('');
  const [loading, setLoading] = useState(true);
  const [profilePicture, setProfilePicture] = useState<string>('defaultProfilePicture.jpg');
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
  const [editingPostId, setEditingPostId] = useState<string | null>(null);
  const [editPostCaption, setEditPostCaption] = useState<string>('');

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
    
        const userData = await getUserById(userId);
        setUser(userData);
    
        if (userData.imageId) {
          const imageSrc = await getUserImageById(userData.imageId);
          setProfilePicture(imageSrc);
        }
        await fetchPosts(userId, 1);
      } catch (error) {
        console.error('Error fetching user data:', error);
      } finally {
        setLoading(false);
      }
    };    

    fetchUserData();
  }, []);

  const fetchPosts = async (userId: string, pageIndex: number) => {
    const paginationRequest: PaginationRequestDto = {
      pageIndex: pageIndex,
      pageSize: 10
    };

    const userPostsResponse: PaginationResponse<ResponsePostModel> = await getPostsByUserId(userId, paginationRequest);
    const userPosts = userPostsResponse.items;
    setPosts((prevPosts) => {
      const newPosts = userPosts.filter(post => !prevPosts.some(prevPost => prevPost.id === post.id));
      return [...prevPosts, ...newPosts];
    });
    setPageIndex(userPostsResponse.pageIndex);
    setTotalPages(userPostsResponse.totalPages);

    const imagesPromises = userPosts.map(async (post) => {
      if (post.imageId) {
        try {
          const imageSrc = await getPostImageById(post.imageId);
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

    setPostImages((prevImages) => ({ ...prevImages, ...imagesMap }));

    const reactionsMap = userPosts.reduce((acc, post) => {
      acc[post.id] = post.reactions.length;
      return acc;
    }, {} as { [key: string]: number });

    const commentsMap = userPosts.reduce((acc, post) => {
      acc[post.id] = post.comments.length;
      return acc;
    }, {} as { [key: string]: number });

    const userReactionsMap = userPosts.reduce((acc, post) => {
      const userReaction = post.reactions.find(r => r.userId === userId);
      if (userReaction) {
        acc[post.id] = ReactionType[userReaction.reactionType];
      }
      return acc;
    }, {} as { [key: string]: string });

    setReactionsCount(reactionsMap);
    setCommentsCount(commentsMap);
    setUserReactions(userReactionsMap);
  };

  const handleReaction = async (postId: string, reactionType: string) => {
    try {
      const token = localStorage.getItem('token');
      if (!token) {
        console.error('Token not found in localStorage');
        return;
      }
  
      const currentUserReactionType = userReactions[postId];
      const reactionTypeId = reactionTypes[reactionType];
      const postIndex = posts.findIndex(post => post.id === postId);
      const reaction = posts[postIndex]?.reactions.find(r => r.userId === user?.id);
  
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

  const handleEditPost = (postId: string, currentCaption: string) => {
    setEditingPostId(postId);
    setEditPostCaption(currentCaption);
  };

  const handleSavePost = async (postId: string) => {
    try {
      const token = localStorage.getItem('token');
      if (!token) return;

      await updatePost(postId, { caption: editPostCaption });
      setEditingPostId(null);
      setPosts(prevPosts => {
        const updatedPosts = [...prevPosts];
        const postIndex = updatedPosts.findIndex(post => post.id === postId);
        if (postIndex !== -1) {
          updatedPosts[postIndex].caption = editPostCaption;
        }
        return updatedPosts;
      });
    } catch (error) {
      console.error('Error saving post:', error);
    }
  };

  const handleDeletePost = async (postId: string) => {
    try {
      await removePost(postId);
      setPosts(prevPosts => prevPosts.filter(post => post.id !== postId));
    } catch (error) {
      console.error('Error deleting post:', error);
    }
  };

  const fetchReactions = async (postId: string, pageIndex: number = 1) => {
    try {
      const token = localStorage.getItem('token');
      if (!token) {
        console.error('Token not found in localStorage');
        return;
      }

      const reactionData = await getPostReactions(postId, pageIndex);
      const formattedReactions: ReactionWithEmoji[] = await Promise.all(reactionData.items.map(async (reaction: ResponseReaction) => {
        const avatar = reaction.imageId ? await getUserImageById(reaction.imageId) : '';
        return {
          userName: reaction.userName,
          avatar,
          reactionType: ReactionType[reaction.reactionType],
          reactionEmoji: getReactionEmoji(ReactionType[reaction.reactionType])
        };
      }));
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
      const token = localStorage.getItem('token');
      if (!token) {
        console.error('Token not found in localStorage');
        return;
      }

      const commentData = await getPostComments(postId, pageIndex, pageSize);
      
      if (!commentData.items || commentData.items.length === 0) {
        setComments([]);
        setCommentTotalPages(1);
      } else {
        const formattedComments: FormattedComment[] = await Promise.all(commentData.items.map(async (comment: ResponseComment) => {
          const avatar = comment.imageId ? await getUserImageById(comment.imageId) : '';
          return {
            id: comment.id,
            userId: comment.userId,
            userName: comment.userName,
            avatar,
            text: comment.text,
            createdAt: comment.createdAt
          };
        }));
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

      setCommentsCount(prev => ({
        ...prev,
        [currentPostId]: (prev[currentPostId] || 0) + 1
      }));

      setNewComment('');
      fetchComments(currentPostId, currentCommentPageIndex);
    } catch (error) {
      console.error('Error adding comment:', error);
    }
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
      const token = localStorage.getItem('token');
      if (!token) return;
  
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
      if (!token || !user) return;
      await fetchPosts(user.id, pageIndex + 1);
    } catch (error) {
      console.error('Error loading more posts:', error);
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
      <UserProfileInfo profilePicture={profilePicture} username={user.userName} bio={user.bio} />
      {posts.map((post) => (
        <Post
          key={post.id}
          post={post}
          profilePicture={profilePicture}
          userReactions={userReactions}
          reactionsCount={reactionsCount}
          commentsCount={commentsCount}
          showReactions={showReactions}
          handleReaction={handleReaction}
          fetchReactions={fetchReactions}
          handleOpenComments={handleOpenComments}
          editingPostId={editingPostId}
          editPostCaption={editPostCaption}
          handleEditPost={handleEditPost}
          handleSavePost={handleSavePost}
          setEditPostCaption={setEditPostCaption}
          handleDeletePost={handleDeletePost}
          postImages={postImages}
          setShowReactions={setShowReactions}
        />
      ))}
      {pageIndex < totalPages && (
        <Button onClick={loadMorePosts} variant="contained" color="primary" style={{ marginTop: '10px' }}>
          Load More
        </Button>
      )}
      <ReactionModal
        open={openReactionModal}
        onClose={handleClose}
        reactions={reactions}
        pageIndex={pageIndex}
        totalPages={totalPages}
        fetchReactions={fetchReactions}
        currentPostId={currentPostId}
      />
      <CommentModal
        open={openCommentModal}
        onClose={handleClose}
        comments={comments}
        newComment={newComment}
        setNewComment={setNewComment}
        handleAddComment={handleAddComment}
        editingCommentId={editingCommentId}
        editCommentText={editCommentText}
        setEditCommentText={setEditCommentText}
        handleUpdateComment={handleUpdateComment}
        handleDeleteComment={handleDeleteComment}
        currentCommentPageIndex={currentCommentPageIndex}
        commentTotalPages={commentTotalPages}
        fetchComments={fetchComments}
        currentPostId={currentPostId}
        setEditingCommentId={setEditingCommentId}
      />
    </div>
  );
};

export default MyUserProfile;
