import apiClient from '../Config/AxiosInterceptor';
import { UserFeed } from '../Models/FeedPostInterface';
import { PaginationResponse } from '../Models/PaginationResponse';

const getUserFeed = async (pageIndex: number = 1, pageSize: number = 10): Promise<PaginationResponse<UserFeed>> => {
    try {
      const response = await apiClient.get('/feeds/user', {
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
