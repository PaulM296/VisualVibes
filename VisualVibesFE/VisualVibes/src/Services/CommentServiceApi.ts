import axios from 'axios';
import { BASE_URL } from '../Config/ApiConfig';
import { ResponseComment } from '../Models/ResponseComment';
import { PaginationResponse } from '../Models/PaginationResponse';

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

export const getPostComments = async (postId: string, token: string, pageIndex: number = 1, pageSize: number = 10): Promise<PaginationResponse<ResponseComment>> => {
  try {
    const response = await axios.get(`${BASE_URL}/comments/post/${postId}`, {
      headers: {
        Authorization: `Bearer ${token}`
      },
      params: {
        PageIndex: pageIndex,
        PageSize: pageSize
      }
    });
    return response.data;
  } catch (error) {
    console.error('Failed to fetch comments:', error);
    throw error;
  }
};