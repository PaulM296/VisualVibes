import React from 'react';
import { Avatar, Button, TextField } from '@mui/material';
import MoreVertMenu from '../MoreVertMenu';
import ReactionButton from '../ReactionButton';
import { ResponsePostModel } from '../../Models/ReponsePostModel';
import { formatPostDate } from '../../Utils/formatPostDateUtil';
import './Post.css'

interface PostProps {
  post: ResponsePostModel;
  profilePicture: string;
  userReactions: { [key: string]: string };
  reactionsCount: { [key: string]: number };
  commentsCount: { [key: string]: number };
  showReactions: { [key: string]: boolean };
  handleReaction: (postId: string, reactionType: string) => void;
  fetchReactions: (postId: string, pageIndex: number) => void;
  handleOpenComments: (postId: string) => void;
  editingPostId: string | null;
  editPostCaption: string;
  handleEditPost: (postId: string, currentCaption: string) => void;
  handleSavePost: (postId: string) => void;
  setEditPostCaption: React.Dispatch<React.SetStateAction<string>>;
  handleDeletePost: (postId: string) => void;
  postImages: { [key: string]: string };
  setShowReactions: React.Dispatch<React.SetStateAction<{ [key: string]: boolean }>>;
}

const Post: React.FC<PostProps> = ({
  post,
  profilePicture,
  userReactions,
  reactionsCount,
  commentsCount,
  showReactions,
  handleReaction,
  fetchReactions,
  handleOpenComments,
  editingPostId,
  editPostCaption,
  handleEditPost,
  handleSavePost,
  setEditPostCaption,
  handleDeletePost,
  postImages,
  setShowReactions
}) => {
  return (
    <div className="feedPost">
      <div className="feedPostWrapper">
        <div className="feedPostTop">
          <div className="feedPostTopLeft">
            <Avatar style={{border: '1px solid #072E33'}} alt={post.userName} src={profilePicture} className="feedPostProfileImg" />
            <div className="feedPostUserDetails">
              <span className="feedPostUsername">{post.userName}</span>
              <span className="feedPostDate">{formatPostDate(post.createdAt)}</span>
            </div>
          </div>
          <div className="feedPostTopRight">
            <MoreVertMenu
              postId={post.id}
              onPostUpdated={() => handleEditPost(post.id, post.caption)}
              onPostDeleted={() => handleDeletePost(post.id)}
              onEdit={() => handleEditPost(post.id, post.caption)}
            />
          </div>
        </div>
        <div className="feedPostCenter">
          {editingPostId === post.id ? (
            <>
              <TextField
                value={editPostCaption}
                onChange={(e) => setEditPostCaption(e.target.value)}
                fullWidth
                multiline
              />
              <Button variant="contained" color="primary" onClick={() => handleSavePost(post.id)}>Save</Button>
              <Button onClick={() => handleEditPost('', '')}>Cancel</Button>
            </>
          ) : (
            <span className="feedPostText" dangerouslySetInnerHTML={{ __html: post.caption }}></span>
          )}
          {post.imageId && postImages[post.id] && (
            <img className="feedPostImg" src={postImages[post.id]} alt="Post image" />
          )}
        </div>
        <div className="feedPostBottom">
          <div className="feedPostBottomLeft">
            <ReactionButton
              postId={post.id}
              userReactions={userReactions}
              reactionsCount={reactionsCount}
              handleReaction={handleReaction}
              fetchReactions={fetchReactions}
              showReactions={showReactions}
              setShowReactions={setShowReactions}
            />
          </div>
          <div className="feedPostBottomRight">
            <span
              className="feedPostCommentText"
              onClick={() => handleOpenComments(post.id)}
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
  );
};

export default Post;
