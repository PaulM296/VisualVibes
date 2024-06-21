import React, { useState, useEffect } from "react";
import {
  Modal,
  Typography,
  Avatar,
  Button,
  IconButton,
  Menu,
  MenuItem,
} from "@mui/material";
import MoreVertIcon from "@mui/icons-material/MoreVert";
import DOMPurify from 'dompurify';
import { FormattedComment } from "../Models/ResponseComment";
import RichTextEditor from "./RichTextEditor/RichTextEditor";
import { getUserIdFromToken } from "../Utils/auth";
import ConfirmationDialog from "./ConfirmationDialog";
import './Comment/Comment.css';

interface CommentModalProps {
  open: boolean;
  onClose: () => void;
  comments: FormattedComment[];
  newComment: string;
  setNewComment: (comment: string) => void;
  handleAddComment: () => void;
  editingCommentId: string | null;
  editCommentText: string;
  setEditCommentText: (text: string) => void;
  handleUpdateComment: () => void;
  handleDeleteComment: (commentId: string) => void;
  handleModerateComment: (commentId: string, isModerated: boolean) => void;
  currentCommentPageIndex: number;
  commentTotalPages: number;
  fetchComments: (postId: string, pageIndex: number) => void;
  currentPostId: string | null;
  setEditingCommentId: (commentId: string | null) => void;
  isAdmin: boolean;
}

const CommentModal: React.FC<CommentModalProps> = ({
  open,
  onClose,
  comments,
  newComment,
  setNewComment,
  handleAddComment,
  editingCommentId,
  editCommentText,
  setEditCommentText,
  handleUpdateComment,
  handleDeleteComment,
  handleModerateComment,
  currentCommentPageIndex,
  commentTotalPages,
  fetchComments,
  currentPostId,
  setEditingCommentId,
  isAdmin,
}) => {
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const [moderatingCommentId, setModeratingCommentId] = useState<string | null>(null);
  const [openConfirmDialog, setOpenConfirmDialog] = useState<boolean>(false);
  const [commentToDelete, setCommentToDelete] = useState<string | null>(null);
  const [newCharCount, setNewCharCount] = useState<number>(0);
  const [editCharCount, setEditCharCount] = useState<number>(0);
  const userId = getUserIdFromToken();
  const charLimit = 300;

  useEffect(() => {
    setNewCharCount(newComment.length);
  }, [newComment]);

  useEffect(() => {
    if (editingCommentId) {
      setEditCharCount(editCommentText.length);
    }
  }, [editCommentText, editingCommentId]);

  const handleMenuOpen = (event: React.MouseEvent<HTMLButtonElement>, commentId: string) => {
    setAnchorEl(event.currentTarget);
    setModeratingCommentId(commentId);
  };

  const handleMenuClose = () => {
    setAnchorEl(null);
    setModeratingCommentId(null);
  };

  const handleEditComment = (commentId: string, text: string) => {
    setEditingCommentId(commentId);
    setEditCommentText(text);
    setEditCharCount(text.length);
  };

  const handleDeleteClick = (commentId: string) => {
    setCommentToDelete(commentId);
    setOpenConfirmDialog(true);
  };

  const handleConfirmDelete = () => {
    if (commentToDelete) {
      handleDeleteComment(commentToDelete);
    }
    setOpenConfirmDialog(false);
    setCommentToDelete(null);
  };

  const handleCancelDelete = () => {
    setOpenConfirmDialog(false);
    setCommentToDelete(null);
  };

  return (
    <Modal open={open} onClose={onClose}>
      <div className="modalContentLarge">
        <Typography variant="h6">Comments</Typography>
        <div className="addCommentContainer">
          <div className="editor-container">
            <RichTextEditor content={newComment} setContent={setNewComment} />
            <Typography variant="body2" color={newCharCount > charLimit ? 'error' : 'textSecondary'}>
              {newCharCount}/{charLimit} characters
            </Typography>
          </div>
          <Button
            variant="contained"
            color="primary"
            onClick={handleAddComment}
            disabled={newCharCount > charLimit}
          >
            Add comment
          </Button>
        </div>
        <div className="commentsContainer">
          {comments.length === 0 && (
            <Typography sx={{ mt: 2 }}>No comments yet.</Typography>
          )}
          {comments.length > 0 &&
            comments.map((comment, index) => (
              <div key={index} className="commentItem comment">
                <Avatar
                  style={{ border: "1px solid #072E33" }}
                  src={comment.avatar}
                  alt={comment.userName}
                  sx={{ margin: "0 10px" }}
                />
                <div className="commentContent">
                  <Typography>{comment.userName}</Typography>
                  {comment.isModerated ? (
                    <Typography
                      variant="h6"
                      color="error"
                      sx={{ fontSize: "18px", fontWeight: "bold" }}
                    >
                      This comment was moderated and is currently under review by one of our administrators!
                    </Typography>
                  ) : (
                    <>
                      {editingCommentId === comment.id ? (
                        <>
                          <RichTextEditor
                            content={editCommentText}
                            setContent={setEditCommentText}
                          />
                          <Typography variant="body2" color={editCharCount > charLimit ? 'error' : 'textSecondary'}>
                            {editCharCount}/{charLimit} characters
                          </Typography>
                          <div className="commentEdit">
                            <Button
                              variant="contained"
                              color="primary"
                              onClick={handleUpdateComment}
                              disabled={editCharCount > charLimit}
                            >
                              Save
                            </Button>
                            <Button onClick={() => setEditingCommentId(null)}>
                              Cancel
                            </Button>
                          </div>
                        </>
                      ) : (
                        <Typography
                          sx={{ marginLeft: "10px", maxWidth: '550px' }}
                          dangerouslySetInnerHTML={{ __html: DOMPurify.sanitize(comment.text) }}
                        ></Typography>
                      )}
                    </>
                  )}
                </div>
                <div className="commentActions">
                  {comment.userId === userId && (
                    <>
                      <Button onClick={() => handleEditComment(comment.id, comment.text)}>
                        Edit
                      </Button>
                      <Button onClick={() => handleDeleteClick(comment.id)}>
                        Delete
                      </Button>
                    </>
                  )}
                  {isAdmin && (
                    <IconButton onClick={(event) => handleMenuOpen(event, comment.id)}>
                      <MoreVertIcon />
                    </IconButton>
                  )}
                  <Menu
                    anchorEl={anchorEl}
                    open={Boolean(anchorEl) && moderatingCommentId === comment.id}
                    onClose={handleMenuClose}
                  >
                    <MenuItem onClick={() => handleModerateComment(comment.id, comment.isModerated)}>
                      {comment.isModerated ? "Unmoderate" : "Moderate"}
                    </MenuItem>
                  </Menu>
                </div>
              </div>
            ))}
          <div className="paginationControls">
            <Button
              disabled={currentCommentPageIndex === 1}
              onClick={() => fetchComments(currentPostId!, currentCommentPageIndex - 1)}
            >
              Previous
            </Button>
            <Typography>
              {currentCommentPageIndex} / {commentTotalPages}
            </Typography>
            <Button
              disabled={currentCommentPageIndex === commentTotalPages}
              onClick={() => fetchComments(currentPostId!, currentCommentPageIndex + 1)}
            >
              Next
            </Button>
          </div>
        </div>
        <ConfirmationDialog
          open={openConfirmDialog}
          title="Delete Comment"
          message="Are you sure you want to delete this comment?"
          onConfirm={handleConfirmDelete}
          onClose={handleCancelDelete}
        />
      </div>
    </Modal>
  );
};

export default CommentModal;
