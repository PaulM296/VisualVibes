import React from "react";
import Home from "./Pages/Home/Home";
import Login from "./Pages/Login/Login";
import { HelmetProvider } from "react-helmet-async";
import { Route, Routes } from "react-router-dom";
import Signup from "./Pages/Signup/Signup";

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
      </Routes>
    </HelmetProvider>
  )
}

export default App
