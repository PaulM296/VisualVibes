import axios from 'axios';
import { BASE_URL } from '../Config/ApiConfig';
import { ResponseComment } from '../Models/ResponseComment';
import { PaginationResponse } from '../Models/PaginationResponse';

const getCommentsCountByPostId = async (postId: string, token: string) => {
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

const getPostComments = async (postId: string, token: string, pageIndex: number = 1, pageSize: number = 10): Promise<PaginationResponse<ResponseComment>> => {
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

const addComment = async (postId: string, text: string, token: string) => {
  try {
    const response = await axios.post(`${BASE_URL}/comments`, {
      postId,
      text
    }, {
      headers: {
        Authorization: `Bearer ${token}`
      }
    });
    return response.data;
  } catch (error) {
    console.error('Failed to create comment:', error);
    throw error;
  }
};

const updateComment = async (commentId: string, text: string, token: string) => {
  try {
    const response = await axios.put(`${BASE_URL}/comments/${commentId}`, { text }, {
      headers: {
        Authorization: `Bearer ${token}`
      }
    });
    return response.data;
  } catch (error) {
    console.error('Failed to update comment:', error);
    throw error;
  }
};

const deleteComment = async (commentId: string, token: string) => {
  try {
    const response = await axios.delete(`${BASE_URL}/comments/${commentId}`, {
      headers: {
        Authorization: `Bearer ${token}`
      }
    });
    return response.data;
  } catch (error) {
    console.error('Failed to delete comment:', error);
    throw error;
  }
};

export { getCommentsCountByPostId, getPostComments, addComment, updateComment, deleteComment };