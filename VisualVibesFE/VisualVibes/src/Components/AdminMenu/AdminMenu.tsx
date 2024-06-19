import React from 'react';
import { Button, Menu, MenuItem } from '@mui/material';

interface AdminMenuProps {
  postId: string;
  isModerated: boolean;
  onModerate: (postId: string) => Promise<void>;
  onUnmoderate: (postId: string) => Promise<void>;
}

const AdminMenu: React.FC<AdminMenuProps> = ({ postId, isModerated, onModerate, onUnmoderate }) => {
  const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);
  
  const handleClick = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleClose = () => {
    setAnchorEl(null);
  };

  const handleModerate = async () => {
    await onModerate(postId);
    handleClose();
  };

  const handleUnmoderate = async () => {
    await onUnmoderate(postId);
    handleClose();
  };

  return (
    <div>
      <Button onClick={handleClick}>⋮</Button>
      <Menu anchorEl={anchorEl} open={Boolean(anchorEl)} onClose={handleClose}>
        <MenuItem onClick={isModerated ? handleUnmoderate : handleModerate}>
          {isModerated ? 'Unmoderate' : 'Moderate'}
        </MenuItem>
      </Menu>
    </div>
  );
};

export default AdminMenu;