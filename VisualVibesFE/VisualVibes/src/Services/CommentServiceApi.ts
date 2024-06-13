import apiClient from '../Config/AxiosInterceptor';
import { ResponseComment } from '../Models/ResponseComment';
import { PaginationResponse } from '../Models/PaginationResponse';

const getCommentsCountByPostId = async (postId: string) => {
  try {
    const response = await apiClient.get(`/comments/post/${postId}/comments/total`);
    return response.data;
  } catch (error) {
    console.error('Failed to fetch comments count:', error);
    throw error;
  }
};

const getPostComments = async (postId: string, pageIndex: number = 1, pageSize: number = 10): Promise<PaginationResponse<ResponseComment>> => {
  try {
    const response = await apiClient.get(`/comments/post/${postId}`, {
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

const addComment = async (postId: string, text: string) => {
  try {
    const response = await apiClient.post('/comments', {
      postId,
      text
    });
    return response.data;
  } catch (error) {
    console.error('Failed to create comment:', error);
    throw error;
  }
};

const updateComment = async (commentId: string, text: string) => {
  try {
    const response = await apiClient.put(`/comments/${commentId}`, { text });
    return response.data;
  } catch (error) {
    console.error('Failed to update comment:', error);
    throw error;
  }
};

const deleteComment = async (commentId: string) => {
  try {
    const response = await apiClient.delete(`/comments/${commentId}`);
    return response.data;
  } catch (error) {
    console.error('Failed to delete comment:', error);
    throw error;
  }
};

export { getCommentsCountByPostId, getPostComments, addComment, updateComment, deleteComment };
