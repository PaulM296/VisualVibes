import React, { createContext, useState, useContext, ReactNode, useCallback, useEffect } from 'react';
import { User, UserContextType } from '../Models/User';
import { Navigate } from 'react-router-dom';
import { getLoggedUserById } from '../Services/UserServiceApi';

export const userContext = createContext<UserContextType | undefined>(undefined);

export const UserProvider = ({ children }: { children: ReactNode }) => {
  const [isBlocked, setIsBlocked] = useState<boolean>(false)
  const [isAdmin, setIsAdmin] = useState<boolean>(false);
  const [user, setUser] = useState<User | undefined>(undefined);
  const isToken = localStorage.getItem('token') ?? false;
  const isLoggedIn = user ?? isToken;

  const fetchUser = useCallback(async () => {
    try {
      const response = await getLoggedUserById();
      setIsBlocked(response.isBlocked)
      if (response.role === 0) {
        setIsAdmin(true);
      }
      setUser(response);
    } catch (error) {
      console.log(error);
    }
  }, []);

  useEffect(() => {
    fetchUser();
  }, []);

  console.log(isLoggedIn);

  return (isLoggedIn ? (
    isBlocked ? (
    <Navigate to="/blocked" />
    ) : <userContext.Provider value={{ user, setUser, isAdmin }}>
      {children}
    </userContext.Provider>) : (
      <Navigate to="/blocked" />
    ));
};

export const useUser = (): UserContextType => {
  const context = useContext(userContext);
  console.log(context);
  if (!context) {
    throw new Error('useUser must be used within a UserProvider');
  }
  return context;
};
