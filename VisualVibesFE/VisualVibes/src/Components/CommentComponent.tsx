import React, { useState } from 'react';
import { Avatar, Button, TextField, Typography, Snackbar } from '@mui/material';
import { deleteComment } from '../Services/CommentServiceApi';
import ConfirmationDialog from './ConfirmationDialog';
import { FormattedComment } from '../Models/ResponseComment';

interface CommentProps {
  comment: FormattedComment;
  onDelete: (commentId: string) => void;
  onUpdate: (commentId: string, text: string) => void;
}

const CommentComponent: React.FC<CommentProps> = ({ comment, onDelete, onUpdate }) => {
  const [editCommentText, setEditCommentText] = useState<string>(comment.text);
  const [editingCommentId, setEditingCommentId] = useState<string | null>(null);
  const [openDialog, setOpenDialog] = useState(false);
  const [snackbarOpen, setSnackbarOpen] = useState(false);

  const handleEditComment = () => {
    setEditingCommentId(comment.id);
  };

  const handleUpdateComment = () => {
    onUpdate(comment.id, editCommentText);
    setEditingCommentId(null);
  };

  const handleDialogOpen = () => {
    setOpenDialog(true);
  };

  const handleDialogClose = () => {
    setOpenDialog(false);
  };

  const handleDeleteComment = async () => {
    try {
      await deleteComment(comment.id);
      onDelete(comment.id);
      setSnackbarOpen(true);
      handleDialogClose();
    } catch (error) {
      console.error('Error deleting comment:', error);
    }
  };

  const handleSnackbarClose = () => {
    setSnackbarOpen(false);
  };

  return (
    <>
      <div className="commentItem">
        <Avatar src={comment.avatar} alt={comment.userName} />
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
              <Button variant="contained" color="primary" onClick={handleUpdateComment}>Save</Button>
              <Button onClick={() => setEditingCommentId(null)}>Cancel</Button>
            </>
          ) : (
            <Typography dangerouslySetInnerHTML={{ __html: comment.text }}></Typography>
          )}
        </div>
        <Button onClick={handleEditComment}>Edit</Button>
        <Button onClick={handleDialogOpen}>Delete</Button>
      </div>
      <ConfirmationDialog
        open={openDialog}
        title="Confirm Delete"
        message="Are you sure you want to delete this comment? This action is permanent!"
        onConfirm={handleDeleteComment}
        onClose={handleDialogClose}
      />
      <Snackbar
        open={snackbarOpen}
        autoHideDuration={6000}
        onClose={handleSnackbarClose}
        message="Comment deleted successfully"
      />
    </>
  );
};

export default CommentComponent;
