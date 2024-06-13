import axios from 'axios';
import { BASE_URL } from '../Config/ApiConfig';
import { UserFeed } from '../Models/FeedPostInterface';


const getUserFeed = async (token: string): Promise<UserFeed> => {
    try {
      const response = await axios.get<UserFeed>(`${BASE_URL}/feeds/user`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
      return response.data;
    } catch (error) {
      console.error('Error fetching user feed:', error);
      throw error;
    }
  };

export { getUserFeed };