import React from 'react';
import { Typography, Box } from '@mui/material';
import { ResponsePostModel } from '../../Models/ReponsePostModel';
import DOMPurify from 'dompurify';
import './AdminPostItem.css';

interface AdminPostItemProps {
  post: ResponsePostModel;
  avatarSrc: string;
}

const AdminPostItem: React.FC<AdminPostItemProps> = ({ post, avatarSrc }) => {
  return (
    <Box className="adminPostItem">
      <Typography
        variant="body1"
        className="adminPostCaption"
        dangerouslySetInnerHTML={{ __html: DOMPurify.sanitize(post.caption) }}
      ></Typography>
      {post.imageId && (
        <img src={avatarSrc} alt="Post" className="adminPostImage" />
      )}
    </Box>
  );
};

export default AdminPostItem;
