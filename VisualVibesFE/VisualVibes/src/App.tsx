import React from "react";
import Home from "./Pages/Home/Home";
import Login from "./Pages/Login/Login";
import { HelmetProvider } from "react-helmet-async";
import { Outlet, Route, Routes } from "react-router-dom";
import Signup from "./Pages/Signup/Signup";
import NotFound from "./Pages/NotFound/NotFound";
import Conversations from "./Pages/Conversations/Conversations";
import Messages from "./Pages/Messages/Messages";
import UserSettings from "./Pages/UserSettings/UserSettings";
import CreatePost from "./Pages/CreatePost/CreatePost";
import MyUserProfile from "./Pages/MyUserProfile/MyUserProfile";
import OtherUserProfile from "./Pages/OtherUserProfile/OtherUserProfile";
import { UserProvider } from "./Hooks/userContext";
import { createTheme, ThemeProvider } from "@mui/material";

const theme = createTheme({
  palette: {
    primary: {
      main: '#0C7075'
    },
    secondary: {
      main: '#072E33',
    },
  },
  typography: {
    fontFamily: "sans-serif"
  }
});

const UserProviderWrapper: React.FC = () => (
  <UserProvider>
    <Outlet />
  </UserProvider>
);

const App: React.FC = () => {
  return (
    <ThemeProvider theme={theme}>
      <HelmetProvider>
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route path="/signup" element={<Signup />} />
        <Route element={<UserProviderWrapper />}>
          <Route path="/" element={<Home />} />
          <Route path="/userSettings" element={<UserSettings />} />
          <Route path="/createPost" element={<CreatePost />} />
          <Route path="/conversations" element={<Conversations />} />
          <Route path="/messages" element={<Messages />} />
          <Route path="/myUserProfile" element={<MyUserProfile />} />
          <Route path="/user/:userId" element={<OtherUserProfile />} />
        </Route>
        <Route path="*" element={<NotFound />} />
      </Routes>
    </HelmetProvider>
    </ThemeProvider>
  );
}

export default App;
