import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { Avatar, Typography, Button, Modal, IconButton, Menu, MenuItem } from '@mui/material';
import MoreVertIcon from '@mui/icons-material/MoreVert';
import Navbar from '../../Components/Navbar/Navbar';
import { differenceInDays, differenceInHours, differenceInMinutes } from 'date-fns';
import { addReaction, deleteReaction, getPostReactions, updateReaction } from '../../Services/ReactionServiceApi';
import { getUserIdFromToken } from '../../Utils/auth';
import { getReactionEmoji } from '../../Utils/getReactionEmoji';
import { ReactionType } from '../../Models/ReactionType';
import { ReactionWithEmoji } from '../../Models/ReactionWithEmoji';
import { FormattedComment, ResponseComment } from '../../Models/ResponseComment';
import { getPostsByUserId, getImageById as getPostImageById, unmoderatePost, moderatePost } from '../../Services/UserPostServiceApi';
import { getImageById as getUserImageById, checkIfFollowing, followUser, unfollowUser, getUserById } from '../../Services/UserServiceApi';
import UserFollowModal from '../UserFollowModal/UserFollowModal';
import { PaginationRequestDto, PaginationResponse } from '../../Models/PaginationResponse';
import { ResponsePostModel } from '../../Models/ReponsePostModel';
import CommentModal from '../CommentModal';
import './OtherUsersProfile.css';
import { User } from '../../Models/User';
import { useUser } from '../../Hooks/userContext';
import { addComment, deleteComment, getPostComments, moderateComment, unmoderateComment, updateComment } from '../../Services/CommentServiceApi';
import { ResponseReaction } from '../../Models/ResponseReaction';
import DOMPurify from 'dompurify';

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

const OtherUsersProfile: React.FC = () => {
    const { userId } = useParams<{ userId: string }>();
    const { isAdmin } = useUser();

    const [user, setUser] = useState<User | null>(null);
    const [posts, setPosts] = useState<ResponsePostModel[]>([]);
    const [postImages, setPostImages] = useState<{ [key: string]: string }>({});
    const [profilePicture, setProfilePicture] = useState<string>('defaultProfilePicture.jpg');
    const [loading, setLoading] = useState(true);
    const [pageIndex, setPageIndex] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
  
    const [showReactions, setShowReactions] = useState<{ [key: string]: boolean }>({});
    const [openReactionModal, setOpenReactionModal] = useState(false);
    const [openCommentModal, setOpenCommentModal] = useState(false);
    const [reactions, setReactions] = useState<ReactionWithEmoji[]>([]);
    const [userReactions, setUserReactions] = useState<{ [key: string]: string }>({});
    const [reactionsCount, setReactionsCount] = useState<{ [key: string]: number }>({});
    const [commentsCount, setCommentsCount] = useState<{ [key: string]: number }>({});
    const [currentPostId, setCurrentPostId] = useState<string | null>(null);
    const [comments, setComments] = useState<FormattedComment[]>([]);
    const [newComment, setNewComment] = useState<string>('');
    const [currentCommentPageIndex, setCurrentCommentPageIndex] = useState(1);
    const [commentTotalPages, setCommentTotalPages] = useState(1);
    const [editingCommentId, setEditingCommentId] = useState<string | null>(null);
    const [editCommentText, setEditCommentText] = useState<string>('');
    const [isFollowing, setIsFollowing] = useState<boolean>(false);
    const [isLoadingFollow, setIsLoadingFollow] = useState<boolean>(true);
    const [openFollowingModal, setOpenFollowingModal] = useState<boolean>(false);
    const [openFollowersModal, setOpenFollowersModal] = useState<boolean>(false);
    const [currentReactionPageIndex, setCurrentReactionPageIndex] = useState(1);
    const [reactionTotalPages, setReactionTotalPages] = useState(1);
    const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
    const [moderatingPostId, setModeratingPostId] = useState<string | null>(null);

    useEffect(() => {
        const fetchUserData = async () => {
            try {
                const token = localStorage.getItem('token');
                if (!token) {
                    console.error('Token not found in localStorage');
                    setLoading(false);
                    return;
                }

                if (!userId) {
                    console.error('User ID is not defined');
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
    }, [userId]);

    const fetchPosts = async (userId: string, pageIndex: number) => {
        const paginationRequest: PaginationRequestDto = {
            pageIndex: pageIndex,
            pageSize: 10
        };

        const userPostsResponse: PaginationResponse<ResponsePostModel> = await getPostsByUserId(userId, paginationRequest);
        const userPosts = userPostsResponse.items;
        setPosts((prevPosts) => {
            const newPosts = userPosts.filter((post: ResponsePostModel) => !prevPosts.some(prevPost => prevPost.id === post.id));
            return [...prevPosts, ...newPosts];
        });
        setPageIndex(userPostsResponse.pageIndex);
        setTotalPages(userPostsResponse.totalPages);

        const imagesPromises = userPosts.map(async (post: ResponsePostModel) => {
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
        const imagesMap = images.reduce((acc: { [key: string]: string }, { postId, imageSrc }: { postId: string, imageSrc: string }) => {
            if (imageSrc) {
                acc[postId] = imageSrc;
            }
            return acc;
        }, {} as { [key: string]: string });

        setPostImages((prevImages) => ({ ...prevImages, ...imagesMap }));
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

    useEffect(() => {
        const fetchFollowingStatus = async () => {
            try {
                const token = localStorage.getItem('token');
                if (!token) {
                    console.error('Token not found in localStorage');
                    return;
                }
                if (!userId) {
                    console.error('User ID is not defined');
                    return;
                }
                const followingStatus = await checkIfFollowing(userId);
                setIsFollowing(followingStatus);
            } catch (error) {
                console.error('Error checking following status:', error);
            } finally {
                setIsLoadingFollow(false);
            }
        };

        fetchFollowingStatus();
    }, [userId]);

    useEffect(() => {
        if (!loading && user && posts.length > 0) {
            const reactionsMap = posts.reduce((acc, post) => {
                acc[post.id] = post.reactions.length;
                return acc;
            }, {} as { [key: string]: number });

            const commentsMap = posts.reduce((acc, post) => {
                acc[post.id] = post.comments.length;
                return acc;
            }, {} as { [key: string]: number });

            const userReactionsMap = posts.reduce((acc, post) => {
                const userReaction = post.reactions.find(r => r.userId === getUserIdFromToken());
                if (userReaction) {
                    acc[post.id] = ReactionType[userReaction.reactionType];
                }
                return acc;
            }, {} as { [key: string]: string });

            setReactionsCount(reactionsMap);
            setCommentsCount(commentsMap);
            setUserReactions(userReactionsMap);
        }
    }, [loading, user, posts, userId]);

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
    
            const currentUserReactionType = userReactions[postId];
            const reactionTypeId = reactionTypes[reactionType];
            const postIndex = posts.findIndex(post => post.id === postId);
            const reaction = posts[postIndex]?.reactions.find(r => r.userId === getUserIdFromToken());
    
            console.log('Current User Reaction Type:', currentUserReactionType);
            console.log('Reaction Type:', reactionType);
    
            const newReactionsCount = { ...reactionsCount };
            const newUserReactions = { ...userReactions };
            const updatedPosts = [...posts];
    
            if (currentUserReactionType) {
                if (currentUserReactionType === reactionType) {
                    if (reaction) {
                        await deleteReaction(reaction.id);
                        newReactionsCount[postId] = (newReactionsCount[postId] || 0) - 1;
                        delete newUserReactions[postId];
                        updatedPosts[postIndex].reactions = updatedPosts[postIndex].reactions.filter(r => r.id !== reaction.id);
                    }
                } else {
                    if (reaction) {
                        await updateReaction(reaction.id, reactionTypeId);
                        newUserReactions[postId] = reactionType;
                        const reactionIndex = updatedPosts[postIndex].reactions.findIndex(r => r.id === reaction.id);
                        updatedPosts[postIndex].reactions[reactionIndex].reactionType = reactionTypeId;
                    }
                }
            } else {
                const newReaction = await addReaction(postId, reactionTypeId);
                newReactionsCount[postId] = (newReactionsCount[postId] || 0) + 1;
                newUserReactions[postId] = reactionType;
                updatedPosts[postIndex].reactions.push(newReaction);
            }
    
            setReactionsCount(newReactionsCount);
            setUserReactions(newUserReactions);
            setPosts(updatedPosts);
    
            console.log('Updated Reactions Count:', newReactionsCount);
            console.log('Updated User Reactions:', newUserReactions);
    
        } catch (error) {
            console.error('Error handling reaction:', error);
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
                    avatar: avatar || '',
                    reactionType: ReactionType[reaction.reactionType],
                    reactionEmoji: getReactionEmoji(ReactionType[reaction.reactionType])
                };
            }));
            setReactions(formattedReactions);
            setCurrentPostId(postId);
            setCurrentReactionPageIndex(reactionData.pageIndex);
            setReactionTotalPages(reactionData.totalPages);
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
                        avatar: avatar || '',
                        text: comment.text,
                        createdAt: comment.createdAt,
                        isModerated: comment.isModerated,
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

    const handleFollow = async () => {
        try {
            const token = localStorage.getItem('token');
            if (!token) {
                console.error('Token not found in localStorage');
                return;
            }

            await followUser(userId!);
            setIsFollowing(true);
        } catch (error) {
            console.error('Error following user:', error);
        }
    };

    const handleUnfollow = async () => {
        try {
            const token = localStorage.getItem('token');
            if (!token) {
                console.error('Token not found in localStorage');
                return;
            }

            await unfollowUser(userId!);
            setIsFollowing(false);
        } catch (error) {
            console.error('Error unfollowing user:', error);
        }
    };

    const handleModeratePost = async (postId: string, isModerated: boolean) => {
        try {
            if (isModerated) {
                await unmoderatePost(postId);
            } else {
                await moderatePost(postId);
            }
            setPosts((prevPosts) =>
                prevPosts.map((post) =>
                    post.id === postId ? { ...post, isModerated: !isModerated } : post
                )
            );
        } catch (error) {
            console.error('Error moderating post:', error);
        }
    };

    const handleModerateComment = async (commentId: string, isModerated: boolean) => {
        try {
            if (isModerated) {
                await unmoderateComment(commentId);
            } else {
                await moderateComment(commentId);
            }
            setComments((prevComments) =>
                prevComments.map((comment) =>
                    comment.id === commentId ? { ...comment, isModerated: !isModerated } : comment
                )
            );
        } catch (error) {
            console.error('Error moderating comment:', error);
        }
    };

    const handleMenuOpen = (event: React.MouseEvent<HTMLButtonElement>, postId: string) => {
        setAnchorEl(event.currentTarget);
        setModeratingPostId(postId);
    };

    const handleMenuClose = () => {
        setAnchorEl(null);
        setModeratingPostId(null);
    };

    if (loading || isLoadingFollow) {
        return <p>Loading...</p>;
    }

    if (!user) {
        return <p>User not found</p>;
    }

    return (
        <div className="otherUserPersonalFeedContainer">
            <Navbar />
            <div className="otherUserProfileContent">
                <div className="otherUserPersonalFeedContainer">
                    <div className="otherUserInfo">
                        <Avatar alt={user.userName} src={profilePicture} className="avatar" style={{ border: '1px solid #072E33', width: '150px', height: '150px' }} />
                        <Typography className="username" style={{ marginLeft: '20px', fontSize: '24px', fontWeight: 'bold' }}>{user.userName}</Typography>
                    </div>
                    <Typography className="bio" style={{ marginTop: '10px', fontSize: '16px', textAlign: 'center', width: '100%' }}>
                        {user.bio}
                    </Typography>
                    <div className="userProfileButtons">
                        <Button
                            variant="contained"
                            color={isFollowing ? "secondary" : "primary"}
                            onClick={isFollowing ? handleUnfollow : handleFollow}
                            style={{ marginRight: '10px' }}
                        >
                            {isFollowing ? "Unfollow" : "Follow"}
                        </Button>
                        <Button
                            variant="contained"
                            color="primary"
                            onClick={() => setOpenFollowingModal(true)}
                            style={{ marginRight: '10px' }}
                        >
                            Following
                        </Button>
                        <Button
                            variant="contained"
                            color="primary"
                            onClick={() => setOpenFollowersModal(true)}
                        >
                            Followers
                        </Button>
                    </div>
                    {posts.map((post) => (
                        <div key={post.id} className="otherUserFeedPost">
                            <div className="otherUserFeedPostWrapper">
                                <div className="otherUserFeedPostTop">
                                    <div className="otherUserFeedPostTopLeft">
                                        <Avatar style={{border: '1px solid #072E33'}} alt={user?.userName} src={profilePicture} className="otherUserFeedPostProfileImg" />
                                        <div className="otherUserFeedPostUserInfo">
                                            <span className="otherUserFeedPostUsername">{user?.userName}</span>
                                            <span className="otherUserFeedPostDate">{formatPostDate(post.createdAt)}</span>
                                        </div>
                                    </div>
                                    {isAdmin && (
                                        <div className="otherUserFeedPostTopRight">
                                            <IconButton onClick={(event) => handleMenuOpen(event, post.id)}>
                                                <MoreVertIcon />
                                            </IconButton>
                                            <Menu
                                                anchorEl={anchorEl}
                                                open={Boolean(anchorEl) && moderatingPostId === post.id}
                                                onClose={handleMenuClose}
                                            >
                                                <MenuItem onClick={() => handleModeratePost(post.id, post.isModerated)}>
                                                    {post.isModerated ? 'Unmoderate' : 'Moderate'}
                                                </MenuItem>
                                            </Menu>
                                        </div>
                                    )}
                                </div>
                                {post.isModerated ? (
                                    <div className="otherUserFeedPostCenter">
                                        <Typography variant="h6" color="error" sx={{ fontSize: '16px', fontWeight: 'bold'}}>
                                            This post did not comply to our policies and has been moderated by one of our administrators!
                                        </Typography>
                                    </div>
                                ) : (
                                    <>
                                        <div className="otherUserFeedPostCenter">
                                            <span className="otherUserFeedPostText" dangerouslySetInnerHTML={{ __html: DOMPurify.sanitize(post.caption) }}></span>
                                            {post.imageId && postImages[post.id] && (
                                                <img className="otherUserFeedPostImg" src={postImages[post.id]} alt="Post image" />
                                            )}
                                        </div>
                                        <div className="otherUserFeedPostBottom">
                                            <div
                                                className="otherUserReactionButton"
                                                onMouseEnter={() => setShowReactions((prev) => ({ ...prev, [post.id]: true }))}
                                                onMouseLeave={() => setShowReactions((prev) => ({ ...prev, [post.id]: false }))}
                                            >
                                                {userReactions[post.id] ? (
                                                    <span className="otherUserReactionOpener" role="img" aria-label={userReactions[post.id]}>{getReactionEmoji(userReactions[post.id])}</span>
                                                ) : (
                                                    <span className="otherUserReactionOpener" role="img" aria-label="thumbs up">👍</span>
                                                )}
                                                {showReactions[post.id] && (
                                                    <div className="otherUserReactionOptions">
                                                        {Object.keys(reactionTypes).map(type => (
                                                            <span key={type}
                                                                className={userReactions[post.id] === type ? 'selected' : ''}
                                                                role="img"
                                                                aria-label={type}
                                                                onClick={() => handleReaction(post.id, type)}>
                                                                {getReactionEmoji(type)}
                                                            </span>
                                                        ))}
                                                    </div>
                                                )}
                                            </div>
                                            <div className="otherUserFeedPostStats"
                                                onMouseEnter={() => setShowReactions((prev) => ({ ...prev, [post.id]: true }))}
                                                onMouseLeave={() => setShowReactions((prev) => ({ ...prev, [post.id]: false }))}
                                            >
                                                <span
                                                    className="otherUserFeedPostReactionCounter"
                                                    onClick={() => fetchReactions(post.id)}
                                                    onMouseEnter={(e) => (e.currentTarget.style.color = '#0C7075')}
                                                    onMouseLeave={(e) => (e.currentTarget.style.color = '#072E33')}
                                                    style={{ cursor: 'pointer' }}
                                                >
                                                    {reactionsCount[post.id] || 0} people reacted
                                                </span>
                                                <span
                                                    className="otherUserFeedPostCommentText"
                                                    onClick={() => handleOpenComments(post.id)}
                                                    onMouseEnter={(e) => (e.currentTarget.style.color = '#0C7075')}
                                                    onMouseLeave={(e) => (e.currentTarget.style.color = '#072E33')}
                                                    style={{ cursor: 'pointer' }}
                                                >
                                                    {commentsCount[post.id] || 0} comments
                                                </span>
                                            </div>
                                        </div>
                                    </>
                                )}
                            </div>
                        </div>
                    ))}
                    {pageIndex < totalPages && (
                        <Button onClick={loadMorePosts} variant="contained" color="primary" style={{ marginTop: '10px' }}>
                            Load More
                        </Button>
                    )}
                </div>
            </div>
            <Modal open={openReactionModal} onClose={handleClose}>
                <div className="otherUserModalContent">
                    <Typography variant="h6">Reactions</Typography>
                    {reactions.length === 0 && (
                        <Typography sx={{ mt: 2 }}>No reactions yet.</Typography>
                    )}
                    {reactions.length > 0 && reactions.map((reaction, index) => (
                        <div key={index} className="otherUserReactionItem">
                            <span>{reaction.reactionEmoji}</span>
                            <Avatar style={{border: '1px solid #072E33'}} src={reaction.avatar} alt={reaction.userName} sx={{ margin: '0 10px' }} />
                            <Typography>{reaction.userName}</Typography>
                        </div>
                    ))}
                    <div className="otherUserPaginationControls">
                        <Button
                            disabled={currentReactionPageIndex === 1}
                            onClick={() => fetchReactions(currentPostId!, currentReactionPageIndex - 1)}
                            style={{ float: 'left' }}
                        >
                            Previous
                        </Button>
                        <Typography>{currentReactionPageIndex} / {reactionTotalPages}</Typography>
                        <Button
                            disabled={currentReactionPageIndex === reactionTotalPages}
                            onClick={() => fetchReactions(currentPostId!, currentReactionPageIndex + 1)}
                            style={{ float: 'right' }}
                        >
                            Next
                        </Button>
                    </div>
                </div>
            </Modal>
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
                handleModerateComment={handleModerateComment}
                currentCommentPageIndex={currentCommentPageIndex}
                commentTotalPages={commentTotalPages}
                fetchComments={fetchComments}
                currentPostId={currentPostId}
                setEditingCommentId={setEditingCommentId}
                isAdmin={isAdmin}
            />
            <UserFollowModal
            userId={userId!}
            type="following"
            open={openFollowingModal}
            onClose={() => setOpenFollowingModal(false)}
        />
        <UserFollowModal
            userId={userId!}
            type="followers"
            open={openFollowersModal}
            onClose={() => setOpenFollowersModal(false)}
        />
        </div>
    );
};

export default OtherUsersProfile;
