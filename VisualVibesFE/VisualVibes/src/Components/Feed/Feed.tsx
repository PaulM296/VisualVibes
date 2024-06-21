import React, { useEffect, useState } from "react";
import {
  Avatar,
  Typography,
  Modal,
  Button,
  IconButton,
  Menu,
  MenuItem,
} from "@mui/material";
import MoreVertIcon from "@mui/icons-material/MoreVert";
import { getUserFeed } from "../../Services/UserFeedServiceApi";
import { getImageById as getUserImageById } from "../../Services/UserServiceApi";
import { getImageById as getPostImageById } from "../../Services/UserPostServiceApi";
import {
  addReaction,
  deleteReaction,
  getPostReactions,
  updateReaction,
} from "../../Services/ReactionServiceApi";
import {
  getPostComments,
  addComment,
  updateComment,
  deleteComment,
  moderateComment,
  unmoderateComment,
} from "../../Services/CommentServiceApi";
import {
  moderatePost,
  unmoderatePost,
} from "../../Services/UserPostServiceApi";
import { FeedPost, UserFeed } from "../../Models/FeedPostInterface";
import { getReactionEmoji } from "../../Utils/getReactionEmoji";
import { ResponseReaction } from "../../Models/ResponseReaction";
import { ReactionType } from "../../Models/ReactionType";
import { ReactionWithEmoji } from "../../Models/ReactionWithEmoji";
import {
  ResponseComment,
  FormattedComment,
} from "../../Models/ResponseComment";
import { getUserIdFromToken } from "../../Utils/auth";
import { PaginationResponse } from "../../Models/PaginationResponse";
import { useUser } from "../../Hooks/userContext";
import CommentModal from "../CommentModal";
import "./Feed.css";
import { reactionTypes } from "../../Utils/const/reactionTypes";
import { formatPostDate } from "../../Utils/formatPostDateUtil";

const Feed: React.FC = () => {
  const { isAdmin } = useUser();
  const [posts, setPosts] = useState<FeedPost[]>([]);
  const [postImages, setPostImages] = useState<{ [key: string]: string }>({});
  const [profileImages, setProfileImages] = useState<{ [key: string]: string }>(
    {}
  );
  const [comments, setComments] = useState<FormattedComment[]>([]);
  const [newComment, setNewComment] = useState<string>("");
  const [loading, setLoading] = useState(true);
  const [showReactions, setShowReactions] = useState<{
    [key: string]: boolean;
  }>({});
  const [openReactionModal, setOpenReactionModal] = useState(false);
  const [openCommentModal, setOpenCommentModal] = useState(false);
  const [reactions, setReactions] = useState<ReactionWithEmoji[]>([]);
  const [userReactions, setUserReactions] = useState<{ [key: string]: string }>(
    {}
  );
  const [pageIndex, setPageIndex] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [reactionsCount, setReactionsCount] = useState<{
    [key: string]: number;
  }>({});
  const [commentsCount, setCommentsCount] = useState<{ [key: string]: number }>(
    {}
  );
  const [currentPostId, setCurrentPostId] = useState<string | null>(null);
  const [currentCommentPageIndex, setCurrentCommentPageIndex] = useState(1);
  const [commentTotalPages, setCommentTotalPages] = useState(1);
  const [editingCommentId, setEditingCommentId] = useState<string | null>(null);
  const [editCommentText, setEditCommentText] = useState<string>("");
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const [moderatingPostId, setModeratingPostId] = useState<string | null>(null);

  useEffect(() => {
    fetchFeedData(1);
  }, []);

  const fetchFeedData = async (pageIndex: number) => {
    try {
      const data: PaginationResponse<UserFeed> = await getUserFeed(
        pageIndex,
        10
      );
      const newPosts = data.items[0].posts;
      setPosts((prevPosts) =>
        pageIndex === 1 ? newPosts : [...prevPosts, ...newPosts]
      );
      setPageIndex(data.pageIndex);
      setTotalPages(data.totalPages);

      const imagesPromises = newPosts.map(async (post) => {
        const postImagePromise = post.postImageId
          ? getPostImageById(post.postImageId)
          : Promise.resolve("");
        const profileImagePromise = post.userProfileImageId
          ? getUserImageById(post.userProfileImageId)
          : Promise.resolve("");

        const [postImageSrc, profileImageSrc] = await Promise.all([
          postImagePromise,
          profileImagePromise,
        ]);

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

      const profileImagesMap = images.reduce(
        (acc, { userProfileImageId, profileImageSrc }) => {
          if (userProfileImageId && profileImageSrc) {
            acc[userProfileImageId] = profileImageSrc;
          }
          return acc;
        },
        {} as { [key: string]: string }
      );

      setPostImages((prevImages) => ({ ...prevImages, ...postImagesMap }));
      setProfileImages((prevImages) => ({
        ...prevImages,
        ...profileImagesMap,
      }));

      const reactionsMap = newPosts.reduce((acc, post) => {
        acc[post.postId] = post.reactions.length;
        return acc;
      }, {} as { [key: string]: number });

      const commentsMap = newPosts.reduce((acc, post) => {
        acc[post.postId] = post.comments.length;
        return acc;
      }, {} as { [key: string]: number });

      const userReactionsMap = newPosts.reduce((acc, post) => {
        const userReaction = post.reactions.find(
          (r) => r.userId === getUserIdFromToken()
        );
        if (userReaction) {
          acc[post.postId] = ReactionType[userReaction.reactionType];
        }
        return acc;
      }, {} as { [key: string]: string });

      setReactionsCount((prev) => ({ ...prev, ...reactionsMap }));
      setCommentsCount((prev) => ({ ...prev, ...commentsMap }));
      setUserReactions((prev) => ({ ...prev, ...userReactionsMap }));
    } catch (error) {
      console.error("Error fetching feed data:", error);
    } finally {
      setLoading(false);
    }
  };

  const handleReaction = async (postId: string, reactionType: string) => {
    try {
      const currentUserReactionType = userReactions[postId];
      const reactionTypeId = reactionTypes[reactionType];
      const postIndex = posts.findIndex((post) => post.postId === postId);
      const reaction = posts[postIndex]?.reactions.find(
        (r) => r.userId === getUserIdFromToken()
      );

      if (currentUserReactionType) {
        if (currentUserReactionType === reactionType) {
          if (reaction) {
            await deleteReaction(reaction.id);
            setReactionsCount((prev) => ({
              ...prev,
              [postId]: prev[postId] - 1,
            }));
            setUserReactions((prev) => {
              const newUserReactions = { ...prev };
              delete newUserReactions[postId];
              return newUserReactions;
            });
            setPosts((prevPosts) => {
              const updatedPosts = [...prevPosts];
              updatedPosts[postIndex].reactions = updatedPosts[
                postIndex
              ].reactions.filter((r) => r.id !== reaction.id);
              return updatedPosts;
            });
          }
        } else {
          if (reaction) {
            await updateReaction(reaction.id, reactionTypeId);
            setUserReactions((prev) => ({ ...prev, [postId]: reactionType }));
            setPosts((prevPosts) => {
              const updatedPosts = [...prevPosts];
              const reactionIndex = updatedPosts[postIndex].reactions.findIndex(
                (r) => r.id === reaction.id
              );
              updatedPosts[postIndex].reactions[reactionIndex].reactionType =
                reactionTypeId;
              return updatedPosts;
            });
          }
        }
      } else {
        const newReaction = await addReaction(postId, reactionTypeId);
        setReactionsCount((prev) => ({
          ...prev,
          [postId]: (prev[postId] || 0) + 1,
        }));
        setUserReactions((prev) => ({ ...prev, [postId]: reactionType }));
        setPosts((prevPosts) => {
          const updatedPosts = [...prevPosts];
          updatedPosts[postIndex].reactions.push(newReaction);
          return updatedPosts;
        });
      }
    } catch (error) {
      console.error("Error handling reaction:", error);
    }
  };

  const fetchReactions = async (postId: string, pageIndex: number = 1) => {
    try {
      const reactionData = await getPostReactions(postId, pageIndex);
      const formattedReactions: ReactionWithEmoji[] = await Promise.all(
        reactionData.items.map(async (reaction: ResponseReaction) => {
          const avatar = reaction.imageId
            ? await getUserImageById(reaction.imageId)
            : "";
          return {
            userName: reaction.userName,
            avatar,
            reactionType: ReactionType[reaction.reactionType],
            reactionEmoji: getReactionEmoji(
              ReactionType[reaction.reactionType]
            ),
          };
        })
      );
      setReactions(formattedReactions);
      setCurrentPostId(postId);
      setPageIndex(pageIndex);
      setTotalPages(reactionData.totalPages);
      setOpenReactionModal(true);
    } catch (error) {
      console.error("Error fetching reactions:", error);
    }
  };

  const fetchComments = async (
    postId: string,
    pageIndex: number = 1,
    pageSize: number = 10
  ) => {
    try {
      const commentData = await getPostComments(postId, pageIndex, pageSize);

      if (!commentData.items || commentData.items.length === 0) {
        setComments([]);
        setCommentTotalPages(1);
      } else {
        const formattedComments: FormattedComment[] = await Promise.all(
          commentData.items.map(async (comment: ResponseComment) => {
            const avatar = comment.imageId
              ? await getUserImageById(comment.imageId)
              : "";
            return {
              id: comment.id,
              userId: comment.userId,
              userName: comment.userName,
              avatar,
              text: comment.text,
              createdAt: comment.createdAt,
              isModerated: comment.isModerated,
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
      console.error("Error fetching comments:", error);
      setComments([]);
      setCommentTotalPages(1);
      setOpenCommentModal(true);
    }
  };

  const handleAddComment = async () => {
    try {
      const token = localStorage.getItem("token");
      if (!token || !currentPostId || !newComment) return;

      await addComment(currentPostId, newComment);

      setCommentsCount((prev) => ({
        ...prev,
        [currentPostId]: (prev[currentPostId] || 0) + 1,
      }));

      setNewComment("");
      fetchComments(currentPostId, currentCommentPageIndex);
    } catch (error) {
      console.error("Error adding comment:", error);
    }
  };

  const handleUpdateComment = async () => {
    try {
      const token = localStorage.getItem("token");
      if (!token || !editingCommentId || !editCommentText) return;

      await updateComment(editingCommentId, editCommentText);
      setEditingCommentId(null);
      setEditCommentText("");
      fetchComments(currentPostId!, currentCommentPageIndex);
    } catch (error) {
      console.error("Error updating comment:", error);
    }
  };

  const handleDeleteComment = async (commentId: string) => {
    try {
      await deleteComment(commentId);
      fetchComments(currentPostId!, currentCommentPageIndex);
    } catch (error) {
      console.error("Error deleting comment:", error);
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
      await fetchFeedData(pageIndex + 1);
    } catch (error) {
      console.error("Error loading more posts:", error);
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
          post.postId === postId ? { ...post, isModerated: !isModerated } : post
        )
      );
    } catch (error) {
      console.error("Error moderating post:", error);
    }
  };

  const handleModerateComment = async (
    commentId: string,
    isModerated: boolean
  ) => {
    try {
      if (isModerated) {
        await unmoderateComment(commentId);
      } else {
        await moderateComment(commentId);
      }
      setComments((prevComments) =>
        prevComments.map((comment) =>
          comment.id === commentId
            ? { ...comment, isModerated: !isModerated }
            : comment
        )
      );
    } catch (error) {
      console.error("Error moderating comment:", error);
    }
  };

  const handleMenuOpen = (
    event: React.MouseEvent<HTMLButtonElement>,
    postId: string
  ) => {
    setAnchorEl(event.currentTarget);
    setModeratingPostId(postId);
  };

  const handleMenuClose = () => {
    setAnchorEl(null);
    setModeratingPostId(null);
  };

  if (!posts.length) {
    return <p>No feed data available</p>;
  }

  return (
    <div className="feed">
      <div className="feedWrapper">
        {loading && "Loading"}
        {!loading && (
          <>
            {posts.map((post) => {
              return (
                <div key={post.postId} className="feedPost">
                  <div
                    className={
                      post.isModerated
                        ? "feedPostWrapperModerated"
                        : "feedPostWrapper"
                    }
                  >
                    <div className="feedPostTop">
                      <div className="feedPostTopLeft">
                        <Avatar
                        style={{border: '1px solid #072E33'}}
                          alt={post.userName}
                          src={
                            profileImages[post.userProfileImageId] ||
                            "defaultProfilePicture.jpg"
                          }
                          className="feedPostProfileImg"
                        />
                        <div className="feedPostUserInfo">
                          <span className="feedPostUsername">
                            {post.userName}
                          </span>
                          <span className="feedPostDate">
                            {formatPostDate(post.createdAt)}
                          </span>
                        </div>
                      </div>
                      {isAdmin && (
                        <div className="feedPostTopRight">
                          <IconButton
                            onClick={(event) =>
                              handleMenuOpen(event, post.postId)
                            }
                          >
                            <MoreVertIcon />
                          </IconButton>
                          <Menu
                            anchorEl={anchorEl}
                            open={
                              Boolean(anchorEl) &&
                              moderatingPostId === post.postId
                            }
                            onClose={handleMenuClose}
                          >
                            <MenuItem
                              onClick={() =>
                                handleModeratePost(
                                  post.postId,
                                  post.isModerated
                                )
                              }
                            >
                              {post.isModerated ? "Unmoderate" : "Moderate"}
                            </MenuItem>
                          </Menu>
                        </div>
                      )}
                    </div>
                    {post.isModerated ? (
                      <div className="feedPostCenter">
                        <Typography
                          variant="h6"
                          color="error"
                          sx={{ fontSize: "16px", fontWeight: "bold" }}
                        >
                          This post did not comply to our policies and has been
                          moderated by one of our administrators!
                        </Typography>
                      </div>
                    ) : (
                      <>
                        <div
                          className="feedPostText"
                          dangerouslySetInnerHTML={{
                            __html: post.caption || "",
                          }}
                        ></div>
                        <div className="feedPostCenter">
                          {post.postImageId && postImages[post.postId] && (
                            <img
                              className="feedPostImg"
                              src={postImages[post.postId]}
                              alt="Post image"
                            />
                          )}
                        </div>
                        <div className="feedPostBottom">
                          <div
                            className="feedPostBottomLeft"
                            onMouseEnter={() =>
                              setShowReactions((prev) => ({
                                ...prev,
                                [post.postId]: true,
                              }))
                            }
                            onMouseLeave={() =>
                              setShowReactions((prev) => ({
                                ...prev,
                                [post.postId]: false,
                              }))
                            }
                          >
                            <div className="reactionButton">
                              {userReactions[post.postId] ? (
                                <span
                                  className="reactionOpener selected"
                                  role="img"
                                  aria-label={userReactions[post.postId]}
                                >
                                  {getReactionEmoji(userReactions[post.postId])}
                                </span>
                              ) : (
                                <span
                                  className="reactionOpener"
                                  role="img"
                                  aria-label="thumbs up"
                                >
                                  👍
                                </span>
                              )}
                              {showReactions[post.postId] && (
                                <div className="reactionOptions">
                                  {Object.keys(reactionTypes).map((type) => (
                                    <span
                                      key={type}
                                      className={
                                        userReactions[post.postId] === type
                                          ? "selected"
                                          : ""
                                      }
                                      role="img"
                                      aria-label={type}
                                      onClick={() =>
                                        handleReaction(post.postId, type)
                                      }
                                    >
                                      {getReactionEmoji(type)}
                                    </span>
                                  ))}
                                </div>
                              )}
                            </div>
                            <span
                              className="feedPostReactionCounter"
                              onClick={() => fetchReactions(post.postId)}
                              onMouseEnter={(e) => (e.currentTarget.style.color = '#0C7075')}
                              onMouseLeave={(e) => (e.currentTarget.style.color = '#072E33')}
                            >
                              {reactionsCount[post.postId] || 0} people reacted
                            </span>
                          </div>
                          <div className="feedPostBottomRight">
                            <span
                              className="feedPostCommentText"
                              onClick={() => handleOpenComments(post.postId)}
                              onMouseEnter={(e) =>
                                (e.currentTarget.style.color = "#0C7075")
                              }
                              onMouseLeave={(e) =>
                                (e.currentTarget.style.color = "#072E33")
                              }
                              style={{ cursor: "pointer" }}
                            >
                              {commentsCount[post.postId] || 0} comments
                            </span>
                          </div>
                        </div>
                      </>
                    )}
                  </div>
                </div>
              );
            })}
            {pageIndex < totalPages && (
              <div className="centerButton">
                <Button
                  onClick={loadMorePosts}
                  variant="contained"
                  color="primary"
                  style={{ margin: "auto", display: "block" }}
                >
                  Load More
                </Button>
              </div>
            )}
          </>
        )}
      </div>

      <Modal open={openReactionModal} onClose={handleClose}>
        <div className="modalContent">
          <Typography variant="h6">Reactions</Typography>
          {reactions.length === 0 && (
            <Typography sx={{ mt: 2 }}>No reactions yet.</Typography>
          )}
          {reactions.length > 0 &&
            reactions.map((reaction, index) => (
              <div key={index} className="reactionItem">
                <span>{reaction.reactionEmoji}</span>
                <Avatar
                  src={reaction.avatar}
                  alt={reaction.userName}
                  sx={{ margin: "0 10px" }}
                />
                <Typography>{reaction.userName}</Typography>
              </div>
            ))}
          <div className="paginationControls">
            <Button
              disabled={pageIndex === 1}
              onClick={() => fetchReactions(currentPostId!, pageIndex - 1)}
              style={{ float: "left" }}
            >
              Previous
            </Button>
            <Typography>
              {pageIndex} / {totalPages}
            </Typography>
            <Button
              disabled={pageIndex === totalPages}
              onClick={() => fetchReactions(currentPostId!, pageIndex + 1)}
              style={{ float: "right" }}
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
    </div>
  );
};

export default Feed;
