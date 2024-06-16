import apiClient from '../Config/AxiosInterceptor';
import { User } from "../Models/User";
import { UserFollowerInterface } from "../Models/UserFollowerInterface";

const getUserById = async (userId: string): Promise<User> => {
    const response = await apiClient.get<User>(`/users/${userId}`);
    return response.data;
};

const getImageById = async (imageId: string): Promise<string> => {
    try {
        const response = await apiClient.get<{ imageSrc: string }>(`/image/${imageId}`);
        return response.data.imageSrc;
    } catch (error) {
        console.error(`Error fetching image with ID ${imageId}:`, error);
        throw error;
    }
};

const searchUsers = async (query: string): Promise<User[]> => {
    const response = await apiClient.get<User[]>('/users/search', {
        params: {
            query
        }
    });
    return response.data;
};

const checkIfFollowing = async (userId: string): Promise<boolean> => {
    const response = await apiClient.get<boolean>(`/users/${userId}/is-following`);
    return response.data;
};

const followUser = async (userId: string): Promise<void> => {
    await apiClient.post('/users/follow', null, {
        params: {
            followingId: userId
        }
    });
};

const unfollowUser = async (userId: string): Promise<void> => {
    await apiClient.post('/users/unfollow', null, {
        params: {
            followingId: userId
        }
    });
};

const getUserFollowers = async (userId: string): Promise<User[]> => {
    const response = await apiClient.get<User[]>(`/users/${userId}/followers`);
    return response.data;
};

const getUserFollowing = async (userId: string): Promise<UserFollowerInterface[]> => {
    const response = await apiClient.get<UserFollowerInterface[]>(`/users/${userId}/following`);
    return response.data;
};

const getLoggedUserById = async (): Promise<User> => {
    const response = await apiClient.get<User>('/users/logged-user');
    return response.data;
};

const updateUser = async (userId: string, formData: FormData): Promise<User> => {
    const response = await apiClient.put(`/users/${userId}`, formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    });
    return response.data;
  };

  const deleteUser = async (userId: string) => {
    const response = await apiClient.delete(`/users/${userId}`);
    return response.data;
  };

export { getUserById, getImageById, searchUsers, checkIfFollowing, followUser, unfollowUser, 
    getUserFollowers, getUserFollowing, getLoggedUserById, updateUser, deleteUser };
