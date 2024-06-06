import React, { useState } from 'react';
import { Helmet } from 'react-helmet-async';
import { Card, CardContent, Avatar, TextField, Button, Box, Typography, Snackbar, Alert } from '@mui/material';
import AddPhotoAlternateRoundedIcon from '@mui/icons-material/AddPhotoAlternateRounded';
import './CreatePost.css';
import Navbar from '../../Components/Navbar/Navbar';
import { ResponsePostModel } from '../../Models/ReponsePostModel';
import UserPostServiceApi from '../../Services/UserPostServiceApi';
import { CreatePostModel } from '../../Models/CreatePostModel';
import { useNavigate } from 'react-router-dom';

const CreatePost: React.FC = () => {
  const [content, setContent] = useState('');
  const [file, setFile] = useState<File | null>(null);
  const [preview, setPreview] = useState<string | null>(null);

  const [snackbarOpen, setSnackbarOpen] = useState(false);
  const [snackbarMessage, setSnackbarMessage] = useState('');
  const [snackbarSeverity, setSnackbarSeverity] = useState<'success' | 'error'>('success');

  const handleCloseSnackbar = () => {
      setSnackbarOpen(false);
  };

  const navigate = useNavigate();


  const handleContentChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setContent(event.target.value);
  };

  const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    if (file) {
      setFile(file);
      const reader = new FileReader();
      reader.onloadend = () => {
        setPreview(reader.result as string);
      };
      reader.readAsDataURL(file);
    }
  };

  const handleSubmit = async () => {
    const token = localStorage.getItem('token');

    if (!token) {
      console.error('No token found');
      return;
    }

    const createPostModel: CreatePostModel = {
      caption: content,
      image: file || undefined,
    };

    try {
      const response: ResponsePostModel = await UserPostServiceApi.createPost(createPostModel, token);
      console.log('Post created:', response);
      setSnackbarSeverity('success');
      setSnackbarMessage('Post created successful');
      setSnackbarOpen(true);

      setTimeout(() => {
        navigate('/userProfile');
      }, 2000);
    } catch (error) {
      console.error('Error creating post:', error);
      setSnackbarSeverity('error');
      setSnackbarMessage('Error creating post');
      setSnackbarOpen(true);
    }
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
              <Box display="flex" alignItems="center">
                <input
                  accept="image/*"
                  style={{ display: 'none' }}
                  id="upload-photo"
                  type="file"
                  onChange={handleFileChange}
                />
                <label htmlFor="upload-photo" style={{ display: 'flex', alignItems: 'center' }}>
                  <Button
                    color="primary"
                    aria-label="upload picture"
                    component="span"
                    startIcon={<AddPhotoAlternateRoundedIcon />}
                    sx={{ textTransform: 'none', borderRadius: '20px' }}
                  >
                    Photo
                  </Button>
                </label>
              </Box>
              <Button variant="contained" color="primary" onClick={handleSubmit}>
                Submit
              </Button>
            </Box>
            {preview && (
              <Box display="flex" justifyContent="center" marginTop={2}>
                <img src={preview} alt="Preview" style={{ maxHeight: '200px', maxWidth: '100%' }} />
              </Box>
            )}
          </CardContent>
        </Card>
      </div>
      <Snackbar
        open={snackbarOpen}
        autoHideDuration={2000}
        onClose={handleCloseSnackbar}
        anchorOrigin={{ vertical: 'top', horizontal: 'center' }}
      >
        <Alert onClose={handleCloseSnackbar} severity={snackbarSeverity}>
          {snackbarMessage}
        </Alert>
      </Snackbar>
    </>
  );
};

export default CreatePost;
