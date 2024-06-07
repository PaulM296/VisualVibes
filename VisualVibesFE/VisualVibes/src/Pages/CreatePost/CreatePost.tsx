import React, { useState, useRef, useEffect } from 'react';
import { Helmet } from 'react-helmet-async';
import { Card, CardContent, Avatar, Button, Box, Typography, Snackbar, Alert, IconButton, ClickAwayListener, Popper } from '@mui/material';
import { AiOutlineBold, AiOutlineItalic, AiOutlineUnderline } from 'react-icons/ai';
import EmojiSmile from '@mui/icons-material/EmojiEmotionsOutlined';
import Picker from '@emoji-mart/react';
import data from '@emoji-mart/data';
import './CreatePost.css';
import Navbar from '../../Components/Navbar/Navbar';
import { ResponsePostModel } from '../../Models/ReponsePostModel';
import { createPost } from '../../Services/UserPostServiceApi';
import { CreatePostModel } from '../../Models/CreatePostModel';
import { useNavigate } from 'react-router-dom';
import AddPhotoAlternateRoundedIcon from '@mui/icons-material/AddPhotoAlternateRounded';

type EmojiType = {
  native: string;
};

const CreatePost: React.FC = () => {
  const [content, setContent] = useState('');
  const [file, setFile] = useState<File | null>(null);
  const [preview, setPreview] = useState<string | null>(null);
  const [snackbarOpen, setSnackbarOpen] = useState(false);
  const [snackbarMessage, setSnackbarMessage] = useState('');
  const [snackbarSeverity, setSnackbarSeverity] = useState<'success' | 'error'>('success');

  const [activeCommand, setActiveCommand] = useState({ bold: false, italic: false, underline: false });

  const handleCloseSnackbar = () => {
    setSnackbarOpen(false);
  };

  const navigate = useNavigate();

  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const open = !!anchorEl;
  const editorRef = useRef<HTMLDivElement>(null);

  const toggleEmojiShowing = (event: React.MouseEvent<HTMLButtonElement>) => {
    if (editorRef.current) {
      editorRef.current.focus();
    }
    event.stopPropagation();
    setAnchorEl(anchorEl ? null : event.currentTarget);
  };

  const emojiSelectClick = (emoji: EmojiType) => {
    document.execCommand('insertText', false, emoji.native);
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
      const response: ResponsePostModel = await createPost(createPostModel, token);
      console.log('Post created:', response);
      setSnackbarSeverity('success');
      setSnackbarMessage('Post created successfully');
      setSnackbarOpen(true);

      setTimeout(() => {
        navigate('/myUserProfile');
      }, 2000);
    } catch (error) {
      console.error('Error creating post:', error);
      setSnackbarSeverity('error');
      setSnackbarMessage('Error creating post');
      setSnackbarOpen(true);
    }
  };

  const isCommandActive = (command: string) => {
    return document.queryCommandState(command);
  };

  const updateActiveCommand = () => {
    setActiveCommand({
      bold: isCommandActive('bold'),
      italic: isCommandActive('italic'),
      underline: isCommandActive('underline'),
    });
  };

  const execCommand = (command: string) => {
    document.execCommand(command);
    updateActiveCommand();
  };

  useEffect(() => {
    const handleSelectionChange = () => {
      updateActiveCommand();
    };

    document.addEventListener('selectionchange', handleSelectionChange);
    return () => {
      document.removeEventListener('selectionchange', handleSelectionChange);
    };
  }, []);

  return (
    <>
      <Helmet>
        <title>Create Post</title>
      </Helmet>
      <Navbar />
      <div className="createPostContainer">
        <Card className="card">
          <CardContent className="cardContent">
            <Box className="postHeader">
              <Avatar src="/path/to/avatar.jpg" alt="User Avatar" />
              <Typography variant="h6" className="postHeaderText" style={{ marginLeft: '16px' }}>
                Share your vibes!
              </Typography>
            </Box>
            <div
              ref={editorRef}
              className="editableDiv"
              contentEditable
              onInput={(e) => setContent(e.currentTarget.innerHTML)}
              onFocus={updateActiveCommand}
            ></div>
            <Box className="toolbar">
              <IconButton
                size="small"
                onClick={() => execCommand('bold')}
                className={activeCommand.bold ? 'active' : ''}
              >
                <AiOutlineBold />
              </IconButton>
              <IconButton
                size="small"
                onClick={() => execCommand('italic')}
                className={activeCommand.italic ? 'active' : ''}
              >
                <AiOutlineItalic />
              </IconButton>
              <IconButton
                size="small"
                onClick={() => execCommand('underline')}
                className={activeCommand.underline ? 'active' : ''}
              >
                <AiOutlineUnderline />
              </IconButton>
              <ClickAwayListener onClickAway={() => setAnchorEl(null)}>
                <Box className="emojiPickerContainer">
                  <IconButton size="small" onClick={toggleEmojiShowing}>
                    <EmojiSmile fontSize="small" />
                  </IconButton>
                  <Popper
                    style={{ zIndex: 1400 }}
                    id="emoji-panel"
                    open={open}
                    anchorEl={anchorEl}
                    placement="bottom-start"
                  >
                    <Picker data={data} onEmojiSelect={emojiSelectClick} />
                  </Popper>
                </Box>
              </ClickAwayListener>
            </Box>
            <Box className="actionButtonsContainer">
              <Box className="uploadContainer">
                <input
                  accept="image/*"
                  className="fileInput"
                  id="upload-photo"
                  type="file"
                  onChange={handleFileChange}
                />
                <label htmlFor="upload-photo" className="uploadLabel">
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
              <Box className="previewContainer">
                <img src={preview} alt="Preview" className="previewImage" />
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
