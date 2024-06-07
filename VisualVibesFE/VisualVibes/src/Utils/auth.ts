export const getUserIdFromToken = (): string | null => {
    const token = localStorage.getItem('token');
    if (!token) {
      console.error('Token not found');
      return null;
    }
  
    try {
      const base64Url = token.split('.')[1];
      const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
      const jsonPayload = decodeURIComponent(atob(base64).split('').map((c) => {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
      }).join(''));
  
      const decodedToken = JSON.parse(jsonPayload);
      return decodedToken.userId;
    } catch (error) {
      console.error('Error decoding token:', error);
      return null;
    }
  };