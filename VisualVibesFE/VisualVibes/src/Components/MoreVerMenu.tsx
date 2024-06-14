import React, { useState } from 'react';
import { Menu, MenuItem, IconButton, Snackbar } from '@mui/material';
import MoreVertRoundedIcon from '@mui/icons-material/MoreVertRounded';
import { removePost } from '../Services/UserPostServiceApi';
import ConfirmationDialog from './ConfirmationDialog';

interface MoreVertMenuProps {
  postId: string;
  onPostUpdated: () => void;
  onPostDeleted: () => void;
  onEdit: () => void;
}

const MoreVertMenu: React.FC<MoreVertMenuProps> = ({ postId, onPostDeleted, onEdit }) => {
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const [openDialog, setOpenDialog] = useState(false);
  const [snackbarOpen, setSnackbarOpen] = useState(false);

  const handleMenuOpen = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleMenuClose = () => {
    setAnchorEl(null);
  };

  const handleUpdatePost = () => {
    onEdit();
    handleMenuClose();
  };

  const handleDialogOpen = () => {
    setOpenDialog(true);
    handleMenuClose();
  };

  const handleDialogClose = () => {
    setOpenDialog(false);
  };

  const handleDeletePost = async () => {
    try {
      await removePost(postId);
      onPostDeleted();
      setSnackbarOpen(true);
      handleDialogClose();
      window.location.reload();
    } catch (error) {
      console.error('Error deleting post:', error);
    }
  };

  const handleSnackbarClose = () => {
    setSnackbarOpen(false);
  };

  return (
    <>
      <IconButton onClick={handleMenuOpen}>
        <MoreVertRoundedIcon style={{ cursor: 'pointer' }} />
      </IconButton>
      <Menu
        anchorEl={anchorEl}
        open={Boolean(anchorEl)}
        onClose={handleMenuClose}
      >
        <MenuItem onClick={handleUpdatePost}>Update Post</MenuItem>
        <MenuItem onClick={handleDialogOpen}>Delete Post</MenuItem>
      </Menu>
      <ConfirmationDialog
        open={openDialog}
        title="Confirm Delete"
        message="Are you sure you want to delete the post? This action is permanent!"
        onConfirm={handleDeletePost}
        onClose={handleDialogClose}
      />
      <Snackbar
        open={snackbarOpen}
        autoHideDuration={6000}
        onClose={handleSnackbarClose}
        message="Post deleted successfully"
      />
    </>
  );
};

export default MoreVertMenu;
