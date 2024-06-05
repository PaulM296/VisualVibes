import React from "react";
import Home from "./Pages/Home/Home";
import Login from "./Pages/Login/Login";
import { HelmetProvider } from "react-helmet-async";
import { Route, Routes } from "react-router-dom";
import Signup from "./Pages/Signup/Signup";
import NotFound from "./Pages/NotFound/NotFound";
import Conversations from "./Pages/Conversations/Conversations";
import Messages from "./Pages/Messages/Messages";
import UserSettings from "./Pages/UserSettings/UserSettings";
import UserProfile from "./Pages/UserProfile/UserProfile";
import CreatePost from "./Pages/CreatePost/CreatePost";

export type userContextType = {
  userName: string;
  setUserName: (value: string) => void;
}

export const userContext = React.createContext({} as userContextType)

const App = () => {
  return (
    <HelmetProvider>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/login" element={<Login />} />
        <Route path="/signup" element={<Signup />} />
        <Route path="/userSettings" element={<UserSettings />} />
        <Route path="/createPost" element={<CreatePost />} />
        <Route path="/conversations" element={<Conversations />} />
        <Route path="/messages" element={<Messages />} />
        <Route path="/userProfile" element={<UserProfile/>} />
        <Route path="*" element={<NotFound/>} />
      </Routes>
    </HelmetProvider>
  )
}

export default App
