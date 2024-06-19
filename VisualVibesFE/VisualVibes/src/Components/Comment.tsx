import React from 'react';
import { TextField, Button, Avatar, Typography, IconButton, Menu, MenuItem } from '@mui/material';
import MoreVertIcon from '@mui/icons-material/MoreVert';
import { getUserIdFromToken } from '../Utils/auth';
import './Comment.css';

interface CommentProps {
  comment: {
    id: string;
    userName: string;
    text: string;
    createdAt: Date;
    userId: string;
    avatar: string;
    isModerated: boolean;
  };
  editingCommentId: string | null;
  editCommentText: string;
  setEditCommentText: React.Dispatch<React.SetStateAction<string>>;
  handleUpdateComment: () => void;
  handleEditComment: (commentId: string | null, text: string) => void;
  handleDeleteComment: (commentId: string) => void;
  handleModerateComment: (commentId: string, isModerated: boolean) => void;
  isAdmin: boolean;
}

const Comment: React.FC<CommentProps> = ({
  comment,
  editingCommentId,
  editCommentText,
  setEditCommentText,
  handleUpdateComment,
  handleEditComment,
  handleDeleteComment,
  handleModerateComment,
  isAdmin,
}) => {
  const isEditing = editingCommentId === comment.id;
  const userId = getUserIdFromToken();
  const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);

  const handleMenuOpen = (event: React.MouseEvent<HTMLButtonElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleMenuClose = () => {
    setAnchorEl(null);
  };

  return (
    <div className="comment">
      <Avatar src={comment.avatar} alt={comment.userName} />
      <div className="commentContent">
        <div className="commentHeader">
          <Typography>{comment.userName}</Typography>
          <span className="commentDate">{new Date(comment.createdAt).toLocaleString()}</span>
          {(comment.userId === userId || isAdmin) && (
            <IconButton onClick={handleMenuOpen}>
              <MoreVertIcon />
            </IconButton>
          )}
          <Menu
            anchorEl={anchorEl}
            open={Boolean(anchorEl)}
            onClose={handleMenuClose}
          >
            {comment.userId === userId && (
              <>
                <MenuItem onClick={() => handleEditComment(comment.id, comment.text)}>Edit</MenuItem>
                <MenuItem onClick={() => handleDeleteComment(comment.id)}>Delete</MenuItem>
              </>
            )}
            {isAdmin && (
              <MenuItem onClick={() => handleModerateComment(comment.id, comment.isModerated)}>
                {comment.isModerated ? 'Unmoderate' : 'Moderate'}
              </MenuItem>
            )}
          </Menu>
        </div>
        {comment.isModerated ? (
          <Typography variant="h6" color="error" sx={{ fontSize: '18px', fontWeight: 'bold' }}>
            This comment was moderated and is currently under review by one of our administrators!
          </Typography>
        ) : isEditing ? (
          <div className="commentEdit">
            <TextField
              value={editCommentText}
              onChange={(e) => setEditCommentText(e.target.value)}
              fullWidth
              multiline
            />
            <Button variant="contained" color="primary" onClick={handleUpdateComment}>Save</Button>
            <Button onClick={() => handleEditComment(null, '')}>Cancel</Button>
          </div>
        ) : (
          <div className="commentText">
            <Typography dangerouslySetInnerHTML={{ __html: comment.text }} />
          </div>
        )}
      </div>
    </div>
  );
};

export default Comment;
