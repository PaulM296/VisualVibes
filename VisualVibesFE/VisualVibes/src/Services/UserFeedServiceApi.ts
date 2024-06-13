import axios from 'axios';
import { BASE_URL } from '../Config/ApiConfig';
import { UserFeed } from '../Models/FeedPostInterface';
import { PaginationResponse } from '../Models/PaginationResponse';


const getUserFeed = async (token: string, pageIndex: number = 1, pageSize: number = 10): Promise<PaginationResponse<UserFeed>> => {
    try {
      const response = await axios.get<PaginationResponse<UserFeed>>(`${BASE_URL}/feeds/user`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
        params: {
            PageIndex: pageIndex,
            PageSize: pageSize
          }
      });
      return response.data;
    } catch (error) {
      console.error('Error fetching user feed:', error);
      throw error;
    }
  };

export { getUserFeed };