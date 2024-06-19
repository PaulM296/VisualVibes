import React from 'react';
import { Avatar, Typography, Box } from '@mui/material';

interface UserProfileInfoProps {
  profilePicture: string;
  username: string;
  bio: string;
}

const UserProfileInfo: React.FC<UserProfileInfoProps> = ({ profilePicture, username, bio }) => {
  return (
    <Box display="flex" flexDirection="column" alignItems="center">
      <Avatar src={profilePicture} alt={username} style={{ width: '150px', height: '150px' }} />
      <Typography variant="h5" style={{ marginTop: '10px', fontWeight: 'bold', color: '#072E33' }}>{username}</Typography>
      <Typography variant="body1" style={{ marginTop: '10px', textAlign: 'center', width: '100%', color: '#072E33' }}>{bio}</Typography>
    </Box>
  );
};

export default UserProfileInfo;