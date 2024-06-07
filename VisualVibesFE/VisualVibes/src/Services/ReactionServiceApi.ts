import axios from 'axios';
import { BASE_URL } from '../Config/ApiConfig';

export const getReactionsCountByPostId = async (postId: string, token: string) => {
  try {
    const response = await axios.get(`${BASE_URL}/reactions/post/${postId}/reactions/total`, {
      headers: {
        Authorization: `Bearer ${token}`
      }
    });
    return response.data;
  } catch (error) {
    console.error('Failed to fetch reactions count:', error);
    throw error;
  }
};