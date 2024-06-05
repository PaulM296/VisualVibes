import React, { useState } from 'react';
import { Helmet } from 'react-helmet-async';
import { Card, CardContent, Avatar, TextField, Button, Box, Typography, IconButton } from '@mui/material';
import AddPhotoAlternateRoundedIcon from '@mui/icons-material/AddPhotoAlternateRounded';
import './CreatePost.css';
import Navbar from '../../Components/Navbar/Navbar';

const CreatePost: React.FC = () => {
  const [content, setContent] = useState('');

  const handleContentChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setContent(event.target.value);
  };

  const handleSubmit = () => {
    console.log('Post submitted:', content);
  };

  return (
    <>
      <Helmet>
        <title>Create Post</title>
      </Helmet>
      <Navbar/>      
      <div className="createPostContainer">
          <Card className="card">
            <CardContent className="cardContent">
              <Box display="flex" alignItems="center" marginBottom={2}>
                <Avatar src="/path/to/avatar.jpg" alt="User Avatar" />
                <Typography variant="h6" marginLeft={2}>
                  Share your vibes!
                </Typography>
              </Box>
              <TextField
                fullWidth
                multiline
                rows={6}
                variant="outlined"
                placeholder="Write something..."
                value={content}
                onChange={handleContentChange}
                className="textField"
              />
              <Box display="flex" justifyContent="space-between" alignItems="center" marginTop={2}>
                <IconButton color="primary" aria-label="upload picture" component="label">
                  <input hidden accept="image/*" type="file" />
                  <AddPhotoAlternateRoundedIcon />
                  <Typography variant="body1" marginLeft={1}>Photo</Typography>
                </IconButton>
                <Button variant="contained" color="primary" onClick={handleSubmit}>
                  Submit
                </Button>
              </Box>
            </CardContent>
          </Card>
        </div>
    </>
  );
};

export default CreatePost;
