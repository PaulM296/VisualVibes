import apiClient from '../Config/AxiosInterceptor';
import { ReactionType } from '../Models/ReactionType';
import { PaginationResponse } from '../Models/PaginationResponse';
import { ResponseReaction } from '../Models/ResponseReaction';

const getReactionsCountByPostId = async (postId: string) => {
  try {
    const response = await apiClient.get(`/reactions/post/${postId}/reactions/total`);
    return response.data;
  } catch (error) {
    console.error('Failed to fetch reactions count:', error);
    throw error;
  }
};

const addReaction = async (postId: string, reactionType: number) => {
  try {
    const payload = {
      postId,
      reactionType,
      timestamp: new Date().toISOString()
    };

    console.log('Payload being sent:', payload);

    const response = await apiClient.post('/reactions', payload);
    return response.data;
  } catch (error) {
    console.error('Failed to add reaction:', error);
    throw error;
  }
};

const getPostReactions = async (postId: string, pageIndex: number = 1, pageSize: number = 10): Promise<PaginationResponse<ResponseReaction>> => {
  try {
    const response = await apiClient.get(`/reactions/post/users/${postId}`, {
      params: {
        PageIndex: pageIndex,
        PageSize: pageSize
      }
    });
    return response.data;
  } catch (error) {
    console.error('Failed to fetch reactions:', error);
    throw error;
  }
};

const getUserReactionByPostId = async (postId: string, userId: string): Promise<ReactionType | null> => {
  try {
    const response = await apiClient.get<{ items: ResponseReaction[] }>(`/reactions/post/users/${postId}`, {
      params: {
        PageIndex: 1,
        PageSize: 10
      }
    });

    const userReaction = response.data.items.find((reaction: ResponseReaction) => reaction.userId === userId);
    return userReaction ? userReaction.reactionType : null;
  } catch (error) {
    console.error('Failed to fetch user reaction:', error);
    throw error;
  }
};

const updateReaction = async (reactionId: string, reactionType: number) => {
  try {
    const response = await apiClient.put(`/reactions/${reactionId}`, { reactionType });
    return response.data;
  } catch (error) {
    console.error('Failed to update reaction:', error);
    throw error;
  }
};

const deleteReaction = async (reactionId: string) => {
  try {
    const response = await apiClient.delete(`/reactions/${reactionId}`);
    return response.data;
  } catch (error) {
    console.error('Failed to delete reaction:', error);
    throw error;
  }
};

export { getReactionsCountByPostId, addReaction, getPostReactions, getUserReactionByPostId, updateReaction, deleteReaction };
