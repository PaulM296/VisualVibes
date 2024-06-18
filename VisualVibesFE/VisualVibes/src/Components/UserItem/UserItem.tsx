import React from 'react';
import { User } from '../../Models/User';
import { Avatar, Button, Card, CardContent, Typography } from '@mui/material';
import './UserItem.css';

interface UserItemProps {
  user: User;
  onBlock: (userId: string) => void;
  onUnblock: (userId: string) => void;
}

const UserItem: React.FC<UserItemProps> = ({ user, onBlock, onUnblock }) => {
  return (
    <Card className="user-item">
      <CardContent>
        <Avatar src={user.profilePicture || ''} alt={user.userName} />
        <Typography variant="h6">{user.userName}</Typography>
        <Typography>{user.email}</Typography>
        <Typography>{user.firstName} {user.lastName}</Typography>
        {user.isBlocked ? (
          <Button variant="contained" color="primary" onClick={() => onUnblock(user.id)}>Unblock</Button>
        ) : (
          <Button variant="contained" color="secondary" onClick={() => onBlock(user.id)}>Block</Button>
        )}
      </CardContent>
    </Card>
  );
};

export default UserItem;
