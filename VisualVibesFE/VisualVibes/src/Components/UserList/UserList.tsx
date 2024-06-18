import React from 'react';
import { User } from '../../Models/User';
import { Button, Avatar, Grid, Typography } from '@mui/material';
import './UserList.css';

interface UserListProps {
  users: User[];
  userImages: { [key: string]: string };
  onBlock: (userId: string) => void;
  onUnblock: (userId: string) => void;
}

const UserList: React.FC<UserListProps> = ({ users, userImages, onBlock, onUnblock }) => {
  return (
    <div className="user-list">
      {users.map((user) => (
        <div key={user.id} className="user-card">
          <Grid container alignItems="center" spacing={2}>
            <Grid item xs={1}>
              <Avatar src={userImages[user.id] || '/default-avatar.png'} />
            </Grid>
            <Grid item xs={2}>
              <Typography variant="body2">{user.userName}</Typography>
            </Grid>
            <Grid item xs={3}>
              <Typography variant="body2">{user.email}</Typography>
            </Grid>
            <Grid item xs={2}>
              <Typography variant="body2">{user.firstName}</Typography>
            </Grid>
            <Grid item xs={2}>
              <Typography variant="body2">{user.lastName}</Typography>
            </Grid>
            <Grid item xs={2} className="user-action">
              {user.isBlocked ? (
                <Button variant="contained" color="secondary" onClick={() => onUnblock(user.id)}>
                  Unblock
                </Button>
              ) : (
                <Button variant="contained" color="primary" onClick={() => onBlock(user.id)}>
                  Block
                </Button>
              )}
            </Grid>
          </Grid>
        </div>
      ))}
    </div>
  );
};

export default UserList;
