import axios from 'axios';
import { BASE_URL } from '../Config/ApiConfig';

export const getCommentsCountByPostId = async (postId: string, token: string) => {
  try {
    const response = await axios.get(`${BASE_URL}/comments/post/${postId}/comments/total`, {
      headers: {
        Authorization: `Bearer ${token}`
      }
    });
    return response.data;
  } catch (error) {
    console.error('Failed to fetch comments count:', error);
    throw error;
  }
};