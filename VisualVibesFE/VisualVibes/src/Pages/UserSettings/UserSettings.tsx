import React, { useState, useEffect } from 'react';
import { Avatar, Button, Container, Box, TextField, IconButton, Typography } from '@mui/material';
import { AddPhotoAlternate, Delete } from '@mui/icons-material';
import { Helmet } from 'react-helmet-async';
import { useUser } from '../../Hooks/userContext';
import { updateUser, deleteUser, getImageById as getUserImageById } from '../../Services/UserServiceApi';
import ConfirmationDialog from '../../Components/ConfirmationDialog';
import './UserSettings.css';

const UserSettings: React.FC = () => {
  const { user, setUser } = useUser();
  const [editable, setEditable] = useState(false);
  const [formData, setFormData] = useState<FormData | null>(null);
  const [localUser, setLocalUser] = useState(user);
  const [openDeleteDialog, setOpenDeleteDialog] = useState(false);
  const [profilePicture, setProfilePicture] = useState<string>('');

  useEffect(() => {
    if (user) {
      setLocalUser(user);
      if (user.imageId) {
        fetchUserProfilePicture(user.imageId);
      }
    }
  }, [user]);

  const fetchUserProfilePicture = async (imageId: string) => {
    try {
      const imageSrc = await getUserImageById(imageId);
      setProfilePicture(imageSrc);
    } catch (error) {
      console.error('Error fetching user profile picture:', error);
    }
  };

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (localUser) {
      setLocalUser({ ...localUser, [e.target.name]: e.target.value });
    }
  };

  const handlePhotoChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files && e.target.files.length > 0) {
      const file = e.target.files[0];
      const newFormData = new FormData();
      newFormData.append('ProfilePicture', file);
      setFormData(newFormData);

      // Create a preview URL for the selected file
      const previewUrl = URL.createObjectURL(file);
      setProfilePicture(previewUrl);
    }
  };

  const handleDeletePhoto = () => {
    if (localUser) {
      setLocalUser({ ...localUser, profilePicture: null });
      setProfilePicture('/default-profile.png');
    }
  };

  const handleSave = async () => {
    if (localUser) {
      const updatedFormData = formData || new FormData();
      updatedFormData.append('UserName', localUser.userName);
      updatedFormData.append('Email', localUser.email);
      updatedFormData.append('FirstName', localUser.firstName);
      updatedFormData.append('LastName', localUser.lastName);
      updatedFormData.append('DateOfBirth', localUser.dateOfBirth.toString());
      updatedFormData.append('Bio', localUser.bio || '');
      updatedFormData.append('Role', localUser.role.toString());

      try {
        const updatedUser = await updateUser(localUser.id, updatedFormData);
        setUser(updatedUser);
        setEditable(false);
      } catch (error) {
        console.error('Failed to update user:', error);
      }
    }
  };

  const handleDeleteUser = async () => {
    if (localUser) {
      try {
        await deleteUser(localUser.id);
        // Clear user context and redirect to login or home page
        setUser(undefined);
        localStorage.removeItem('token');
        window.location.href = '/login'; // Adjust this path as needed
      } catch (error) {
        console.error('Failed to delete user:', error);
      }
    }
  };

  const handleOpenDeleteDialog = () => {
    setOpenDeleteDialog(true);
  };

  const handleCloseDeleteDialog = () => {
    setOpenDeleteDialog(false);
  };

  if (!localUser) {
    return <p>Loading...</p>;
  }

  return (
    <>
      <Helmet>
        <title>User Settings</title>
      </Helmet>
      <Container className="user-settings">
        <Box display="flex" alignItems="center" justifyContent="space-between" width="100%">
          <Box position="relative">
            <Avatar
              src={profilePicture || '/default-profile.png'}
              sx={{ width: 100, height: 100 }}
            />
            {editable && (
              <>
                <IconButton
                  color="primary"
                  aria-label="upload picture"
                  component="label"
                  style={{ position: 'absolute', top: 0, right: 0 }}
                >
                  <input hidden accept="image/*" type="file" onChange={handlePhotoChange} />
                  <AddPhotoAlternate />
                </IconButton>
                <IconButton
                  color="secondary"
                  aria-label="delete picture"
                  onClick={handleDeletePhoto}
                  style={{ position: 'absolute', top: 0, left: 0 }}
                >
                  <Delete />
                </IconButton>
              </>
            )}
          </Box>
          <Typography variant="h4" style={{ marginLeft: '20px', flexGrow: 1 }}>
            My Settings
          </Typography>
        </Box>
        <Box mt={2} width="80%">
          <TextField
            label="First name"
            name="firstName"
            value={localUser.firstName}
            onChange={handleInputChange}
            fullWidth
            margin="normal"
            InputProps={{
              readOnly: !editable,
            }}
            style={{ width: '80%' }}
          />
          <TextField
            label="Last name"
            name="lastName"
            value={localUser.lastName}
            onChange={handleInputChange}
            fullWidth
            margin="normal"
            InputProps={{
              readOnly: !editable,
            }}
            style={{ width: '80%' }}
          />
          <TextField
            label="Email"
            name="email"
            value={localUser.email}
            onChange={handleInputChange}
            fullWidth
            margin="normal"
            InputProps={{
              readOnly: !editable,
            }}
            style={{ width: '80%' }}
          />
          <TextField
            label="Username"
            name="userName"
            value={localUser.userName}
            onChange={handleInputChange}
            fullWidth
            margin="normal"
            InputProps={{
              readOnly: !editable,
            }}
            style={{ width: '80%' }}
          />
          <TextField
            label="Bio"
            name="bio"
            value={localUser.bio || ''}
            onChange={handleInputChange}
            fullWidth
            margin="normal"
            multiline
            rows={4}
            InputProps={{
              readOnly: !editable,
            }}
            style={{ width: '80%' }}
          />
          <Box display="flex" flexDirection="row" mt={2}>
            {editable ? (
              <>
                <Button variant="contained" color="primary" onClick={handleSave}>
                  Save
                </Button>
                <Button onClick={() => setEditable(false)} style={{ marginLeft: '8px' }}>
                  Cancel
                </Button>
              </>
            ) : (
              <>
                <Button variant="contained" color="primary" onClick={() => setEditable(true)}>
                  Edit
                </Button>
                <Button
                  variant="contained"
                  color="secondary"
                  onClick={handleOpenDeleteDialog}
                  style={{ marginLeft: '8px' }}
                >
                  Delete Account
                </Button>
              </>
            )}
          </Box>
        </Box>
        <ConfirmationDialog
          open={openDeleteDialog}
          title="Delete Account"
          message="Are you sure you want to delete your account? This action is permanent and cannot be undone."
          onConfirm={handleDeleteUser}
          onClose={handleCloseDeleteDialog}
        />
      </Container>
    </>
  );
};

export default UserSettings;
