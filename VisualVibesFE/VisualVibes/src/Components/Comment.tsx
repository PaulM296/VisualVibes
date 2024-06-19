import React from 'react';
import { TextField, Button, Avatar, Typography } from '@mui/material';
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
  };
  editingCommentId: string | null;
  editCommentText: string;
  setEditCommentText: React.Dispatch<React.SetStateAction<string>>;
  handleUpdateComment: () => void;
  handleEditComment: (commentId: string | null, text: string) => void;
  handleDeleteComment: (commentId: string) => void;
}

const Comment: React.FC<CommentProps> = ({
  comment,
  editingCommentId,
  editCommentText,
  setEditCommentText,
  handleUpdateComment,
  handleEditComment,
  handleDeleteComment,
}) => {
  const isEditing = editingCommentId === comment.id;

  return (
    <div className="comment">
      <Avatar src={comment.avatar} alt={comment.userName} />
      <div className="commentContent">
        <div className="commentHeader">
          <Typography>{comment.userName}</Typography>
          <span className="commentDate">{new Date(comment.createdAt).toLocaleString()}</span>
        </div>
        {isEditing ? (
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
            {comment.userId === getUserIdFromToken() && (
              <div className="commentActions">
                <Button onClick={() => handleEditComment(comment.id, comment.text)}>Edit</Button>
                <Button onClick={() => handleDeleteComment(comment.id)}>Delete</Button>
              </div>
            )}
          </div>
        )}
      </div>
    </div>
  );
};

export default Comment;
