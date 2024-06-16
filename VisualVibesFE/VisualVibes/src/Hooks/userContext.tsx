import React, { createContext, useState, useContext, ReactNode, useCallback, useEffect } from 'react';
import { User, UserContextType } from '../Models/User';
import { Navigate } from 'react-router-dom';
import { getLoggedUserById } from '../Services/UserServiceApi';

export const userContext = createContext<UserContextType | undefined>(undefined);

export const UserProvider = ({ children } : { children: ReactNode }) => {
  const [user, setUser] = useState<User | undefined>(undefined);
  const isToken = localStorage.getItem('token') ?? false;
  const isLoggedIn = user ?? isToken;

  const fetchUser = useCallback(async () => {
    try {
        const response = await getLoggedUserById();
        setUser(response);
    } catch (error) {
        console.log(error);
    }
  }, []);

  useEffect(() => {
    fetchUser();
  }, []);

  return (isLoggedIn ? 
    <userContext.Provider value={{ user, setUser }}>
      {children}
    </userContext.Provider>
    : <Navigate to="/login" />
  );
};

export const useUser = (): UserContextType => {
    const context = useContext(userContext);
    if (!context) {
      throw new Error('useUser must be used within a UserProvider');
    }
    return context;
  };
