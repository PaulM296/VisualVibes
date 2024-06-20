import React from 'react';
import { Avatar, Typography, Box } from '@mui/material';
import { ResponsePostModel } from '../../Models/ReponsePostModel';
import './AdminPostItem.css';

interface AdminPostItemProps {
  post: ResponsePostModel;
  avatarSrc: string;
}

const AdminPostItem: React.FC<AdminPostItemProps> = ({ post, avatarSrc }) => {
  return (
    <Box className="adminPostItem">
      <Box className="adminPostHeader">
        <Avatar style ={{border: "1px solid black"}}src={avatarSrc} alt={post.userName} />
        <Box className="adminPostUserInfo">
          <Typography className="adminPostUserName">
            {post.userName}
          </Typography>
          <Typography variant="body2" className="adminPostDate">
            {new Date(post.createdAt).toLocaleString()}
          </Typography>
        </Box>
      </Box>
      <Typography variant="body1" className="adminPostCaption">
        {post.caption}
      </Typography>
      {post.imageId && (
        <img src={avatarSrc} alt="Post" className="adminPostImage" />
      )}
    </Box>
  );
};

export default AdminPostItem;
