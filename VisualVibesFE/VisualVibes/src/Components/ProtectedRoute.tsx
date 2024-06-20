import React from 'react';
import { Navigate, Outlet } from 'react-router-dom';
import { useUser } from '../Hooks/userContext'; 

interface ProtectedRouteProps {
  requiredRole: number;
}

const ProtectedRoute: React.FC<ProtectedRouteProps> = ({ requiredRole }) => {
  const { user } = useUser();

  if (user && user.role !== requiredRole) {
    return <Navigate to="/forbidden" />;
  }

  return <Outlet />;
};
 
export default ProtectedRoute;