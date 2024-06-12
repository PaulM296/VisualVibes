import React, { useEffect, useState } from 'react';
import { AppBar, Avatar, Badge, Box, IconButton, InputBase, Menu, MenuItem, Toolbar, Typography, alpha, styled, CircularProgress } from '@mui/material';
import MailIcon from '@mui/icons-material/Mail';
import NotificationsIcon from '@mui/icons-material/Notifications';
import MenuIcon from '@mui/icons-material/Menu';
import SearchIcon from '@mui/icons-material/Search';
import MoreIcon from '@mui/icons-material/MoreVert';
import SettingsRoundedIcon from '@mui/icons-material/SettingsRounded';
import AccountCircleRoundedIcon from '@mui/icons-material/AccountCircleRounded';
import AddCircleRoundedIcon from '@mui/icons-material/AddCircleRounded';
import LogoutRoundedIcon from '@mui/icons-material/LogoutRounded';
import { useLocation, useNavigate } from 'react-router-dom';
import { getUserIdFromToken } from '../../Utils/auth';
import { getImageById, getUserById, searchUsers } from '../../Services/UserServiceApi';
import { User } from '../../Models/User';

const Search = styled('div')(({ theme }) => ({
  position: 'relative',
  borderRadius: theme.shape.borderRadius * 5,
  backgroundColor: alpha(theme.palette.common.white, 0.9),
  '&:hover': {
    backgroundColor: alpha(theme.palette.common.white, 1),
  },
  marginRight: theme.spacing(2),
  marginLeft: 0,
  width: '100%',
  [theme.breakpoints.up('sm')]: {
    marginLeft: theme.spacing(3),
    width: 'auto',
  },
}));

const SearchIconWrapper = styled('div')(({ theme }) => ({
  padding: theme.spacing(0, 2),
  height: '100%',
  position: 'absolute',
  pointerEvents: 'none',
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'center',
  color: 'black',
}));

const StyledInputBase = styled(InputBase)(({ theme }) => ({
  color: 'black',
  '& .MuiInputBase-input': {
    padding: theme.spacing(1, 1, 1, 0),
    paddingLeft: `calc(1em + ${theme.spacing(4)})`,
    transition: theme.transitions.create('width'),
    width: '100%',
    [theme.breakpoints.up('md')]: {
      width: '40ch',
    },
  },
}));

const UserSearch = styled('div')(({ theme }) => ({
  position: 'absolute',
  top: '100%',
  left: 0,
  right: 0,
  zIndex: 1,
  backgroundColor: 'white',
  boxShadow: '0 2px 4px rgba(0, 0, 0, 0.2)',
  borderRadius: '4px',
  marginTop: '2px',
  padding: theme.spacing(1),
  maxHeight: '300px',
  overflowY: 'auto',
  color: 'black',
  '& .MuiMenuItem-root': {
    color: 'black',
  },
  '& p': {
    margin: 0,
    paddingLeft: theme.spacing(2),
    color: 'black',
  },
}));

const Navbar = () => {
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const [mobileMoreAnchorEl, setMobileMoreAnchorEl] = useState<null | HTMLElement>(null);
  const [profilePicture, setProfilePicture] = useState<string>('defaultProfilePicture.jpg');
  const [searchQuery, setSearchQuery] = useState<string>('');
  const [searchResults, setSearchResults] = useState<User[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string>('');
  const navigate = useNavigate();
  const location = useLocation();

  const isMenuOpen = Boolean(anchorEl);
  const isMobileMenuOpen = Boolean(mobileMoreAnchorEl);

  useEffect(() => {
    const fetchUserData = async () => {
      try {
        const token = localStorage.getItem('token');
        if (!token) {
          console.error('Token not found in localStorage');
          return;
        }

        const userId = getUserIdFromToken();
        if (!userId) {
          console.error('User ID not found in token');
          return;
        }

        const userData = await getUserById(userId, token);
        if (userData.imageId) {
          const imageSrc = await getImageById(userData.imageId, token);
          setProfilePicture(imageSrc);
        }
      } catch (error) {
        console.error('Error fetching user data:', error);
      }
    };

    fetchUserData();
  }, []);

  const handleProfileMenuOpen = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleMobileMenuClose = () => {
    setMobileMoreAnchorEl(null);
  };

  const handleMenuClose = () => {
    setAnchorEl(null);
    handleMobileMenuClose();
  };

  const handleMobileMenuOpen = (event: React.MouseEvent<HTMLElement>) => {
    setMobileMoreAnchorEl(event.currentTarget);
  };

  const handleUserProfileRedirection = () => {
    navigate('/myUserProfile');
  };

  const handleUserSettingsRedirection = () => {
    navigate('/userSettings');
  };

  const handleCreatePostRedirection = () => {
    navigate('/createPost');
  };

  const handleLogout = () => {
    localStorage.removeItem('token');
    navigate('/login');
  };

  const handleSearchChange = async (event: React.ChangeEvent<HTMLInputElement>) => {
    const query = event.target.value;
    setSearchQuery(query);

    if (query.length > 0) {
      setLoading(true);
      setError('');
      try {
        const token = localStorage.getItem('token');
        if (!token) {
          throw new Error('User not authenticated');
        }
        const searchResults = await searchUsers(query, token);
        const usersWithImages = await Promise.all(searchResults.map(async (user) => {
          const imageSrc = await getImageById(user.imageId, token);
          return { ...user, profilePicture: imageSrc };
        }));
        setSearchResults(usersWithImages);
      } catch (err) {
        setError('Error fetching users.');
        console.error(err);
      } finally {
        setLoading(false);
      }
    } else {
      setSearchResults([]);
    }
  };

  const menuId = 'primary-search-account-menu';
  const renderMenu = (
    <Menu
      anchorEl={anchorEl}
      anchorOrigin={{
        vertical: 'top',
        horizontal: 'right',
      }}
      id={menuId}
      keepMounted
      transformOrigin={{
        vertical: 'top',
        horizontal: 'right',
      }}
      open={isMenuOpen}
      onClose={handleMenuClose}
    >
      <MenuItem onClick={handleUserProfileRedirection}>
        <AccountCircleRoundedIcon sx={{ marginRight: 2 }} />
        My Profile
      </MenuItem>
      <MenuItem onClick={handleUserSettingsRedirection}>
        <SettingsRoundedIcon sx={{ marginRight: 2 }} />
        Settings
      </MenuItem>
      <MenuItem onClick={handleCreatePostRedirection}>
        <AddCircleRoundedIcon sx={{ marginRight: 2 }} />
        Create Post
      </MenuItem>
      <MenuItem onClick={handleLogout}>
        <LogoutRoundedIcon sx={{ marginRight: 2 }}/>
        Logout
      </MenuItem>
    </Menu>
  );

  const mobileMenuId = 'primary-search-account-menu-mobile';
  const renderMobileMenu = (
    <Menu
      anchorEl={mobileMoreAnchorEl}
      anchorOrigin={{
        vertical: 'top',
        horizontal: 'right',
      }}
      id={mobileMenuId}
      keepMounted
      transformOrigin={{
        vertical: 'top',
        horizontal: 'right',
      }}
      open={isMobileMenuOpen}
      onClose={handleMobileMenuClose}
    >
      <MenuItem>
        <IconButton size="large" aria-label="show 4 new mails" color="inherit">
          <Badge badgeContent={4} color="error">
            <MailIcon />
          </Badge>
        </IconButton>
        <p>Messages</p>
      </MenuItem>
      <MenuItem>
        <IconButton
          size="large"
          aria-label="show 17 new notifications"
          color="inherit"
        >
          <Badge badgeContent={17} color="error">
            <NotificationsIcon />
          </Badge>
        </IconButton>
        <p>Notifications</p>
      </MenuItem>
      <MenuItem onClick={handleProfileMenuOpen}>
        <IconButton
          size="large"
          aria-label="account of current user"
          aria-controls="primary-search-account-menu"
          aria-haspopup="true"
          color="inherit"
        >
          <Avatar src={profilePicture} />
        </IconButton>
        <p>Profile</p>
      </MenuItem>
    </Menu>
  );

  return (
    <Box sx={{ flexGrow: 1 }}>
      <AppBar position="fixed">
        <Toolbar>
          <IconButton
            size="large"
            edge="start"
            color="inherit"
            aria-label="open drawer"
            sx={{ mr: 2 }}
          >
            <MenuIcon />
          </IconButton>
          <Typography
            variant="h5"
            component="div"
            color="inherit"
            sx={{
              display: { xs: 'none', sm: 'block' },
              textTransform: 'none',
              borderRadius: '35px',
              color: 'white',
              '&:hover': {
                backgroundColor: location.pathname === '/' ? 'inherit' : 'rgba(255, 255, 255, 0.08)',
              },
              cursor: location.pathname === '/' ? 'default' : 'pointer',
            }}
            onClick={location.pathname !== '/' ? () => navigate('/') : undefined}
          >
            VisualVibes
          </Typography>
          <Box sx={{ flexGrow: 1 }} />
          <Search>
            <SearchIconWrapper>
              <SearchIcon />
            </SearchIconWrapper>
            <StyledInputBase
              placeholder="Search for a user or friend"
              inputProps={{ 'aria-label': 'search' }}
              value={searchQuery}
              onChange={handleSearchChange}
            />
            {loading && <CircularProgress size={24} sx={{ position: 'absolute', right: 10, top: '50%', transform: 'translateY(-50%)' }} />}
            {searchResults.length > 0 && (
              <UserSearch>
                {searchResults.map(user => (
                  <MenuItem key={user.id} onClick={() => navigate(`/user/${user.id}`)}>
                    <Avatar src={user.profilePicture || undefined} alt={`${user.userName}'s profile`}>
                      {user.userName.charAt(0)}
                    </Avatar>
                    <p>{user.userName} ({user.firstName} {user.lastName})</p>
                  </MenuItem>
                ))}
              </UserSearch>
            )}
            {error && <p>{error}</p>}
          </Search>
          <Box sx={{ flexGrow: 1 }} />
          <Box sx={{ display: { xs: 'none', md: 'flex' } }}>
            <IconButton size="large" aria-label="show 4 new mails" color="inherit">
              <Badge badgeContent={10} color="error">
                <MailIcon />
              </Badge>
            </IconButton>
            <IconButton
              size="large"
              aria-label="show 17 new notifications"
              color="inherit"
            >
              <Badge badgeContent={17} color="error">
                <NotificationsIcon />
              </Badge>
            </IconButton>
            <IconButton
              size="large"
              edge="end"
              aria-label="account of current user"
              aria-controls={menuId}
              aria-haspopup="true"
              onClick={handleProfileMenuOpen}
              color="inherit"
            >
              <Avatar src={profilePicture} />
            </IconButton>
          </Box>
          <Box sx={{ display: { xs: 'flex', md: 'none' } }}>
            <IconButton
              size="large"
              aria-label="show more"
              aria-controls={mobileMenuId}
              aria-haspopup="true"
              onClick={handleMobileMenuOpen}
              color="inherit"
            >
              <MoreIcon />
            </IconButton>
          </Box>
        </Toolbar>
      </AppBar>
      {renderMobileMenu}
      {renderMenu}
    </Box>
  );
};

export default Navbar;
