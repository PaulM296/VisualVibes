import axios from "axios";
import { BASE_URL } from "../Config/ApiConfig";
import { User } from "../Models/User";
import { UserFollowerInterface } from "../Models/UserFollowerInterface";

const getUserById = async (userId: string, token: string): Promise<User> => {
    const response = await axios.get<User>(`${BASE_URL}/users/${userId}`, {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    });
    return response.data;
};

const getImageById = async (imageId: string, token: string): Promise<string> => {
    try {
        const response = await axios.get<{ imageSrc: string }>(`${BASE_URL}/image/${imageId}`, {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });
        return response.data.imageSrc;
    } catch (error) {
        console.error(`Error fetching image with ID ${imageId}:`, error);
        throw error;
    }
};

const searchUsers = async (query: string, token: string): Promise<User[]> => {
    const response = await axios.get<User[]>(`${BASE_URL}/users/search`, {
        headers: {
            'Authorization': `Bearer ${token}`
        },
        params: {
            query
        }
    });
    return response.data;
}

const checkIfFollowing = async (userId: string, token: string): Promise<boolean> => {
    const response = await axios.get<boolean>(`${BASE_URL}/users/${userId}/is-following`, {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    });
    return response.data;
};

const followUser = async (userId: string, token: string): Promise<void> => {
    await axios.post(`${BASE_URL}/users/follow`, null, {
        headers: {
            'Authorization': `Bearer ${token}`
        },
        params: {
            followingId: userId
        }
    });
};

const unfollowUser = async (userId: string, token: string): Promise<void> => {
    await axios.post(`${BASE_URL}/users/unfollow`, null, {
        headers: {
            'Authorization': `Bearer ${token}`
        },
        params: {
            followingId: userId
        }
    });
};

const getUserFollowers = async (userId: string, token: string): Promise<User[]> => {
    const response = await axios.get<User[]>(`${BASE_URL}/users/${userId}/followers`, {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    });
    return response.data;
}

const getUserFollowing = async (userId: string, token: string): Promise<UserFollowerInterface[]> => {
    const response = await axios.get<UserFollowerInterface[]>(`${BASE_URL}/users/${userId}/following`, {
      headers: {
        'Authorization': `Bearer ${token}`
      }
    });
    return response.data;
  };
  

export { getUserById, getImageById, searchUsers, checkIfFollowing, followUser, unfollowUser, getUserFollowers, getUserFollowing };