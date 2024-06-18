import React from 'react';
import { Navigate, Outlet } from 'react-router-dom';
import { useUser } from '../Hooks/userContext'; 

interface ProtectedRouteProps {
    isAdminRoute?: boolean;
  }

const ProtectedRoute: React.FC<ProtectedRouteProps> = ({ isAdminRoute }) => {
  const { user, isAdmin } = useUser();
 
  if (!user) {
    return <Navigate to="/" replace />;
  }
 
  if (isAdminRoute && !isAdmin) {
    return <Navigate to="/" replace />;
  }
 
  return <Outlet />;
};
 
export default ProtectedRoute;