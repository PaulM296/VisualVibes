import axios from 'axios';
import { BASE_URL } from '../Config/ApiConfig';
import { ReactionType } from '../Models/ReactionType';
import { PaginationResponse } from '../Models/PaginationResponse';
import { ResponseReaction } from '../Models/ResponseReaction';

const getReactionsCountByPostId = async (postId: string, token: string) => {
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

const addReaction = async (postId: string, reactionType: number, token: string) => {
  try {
    const payload = {
      postId,
      reactionType,
      timestamp: new Date().toISOString()
    };

    console.log('Payload being sent:', payload);

    const response = await axios.post(
      `${BASE_URL}/reactions`,
      payload,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
    return response.data;
  } catch (error) {
    console.error('Failed to add reaction:', error);
    throw error;
  }
};

const getPostReactions = async (postId: string, token: string, pageIndex: number = 1, pageSize: number = 10): Promise<PaginationResponse<ResponseReaction>> => {
  try {
    const response = await axios.get<PaginationResponse<ResponseReaction>>(`${BASE_URL}/reactions/post/users/${postId}`, {
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
    console.error('Failed to fetch reactions:', error);
    throw error;
  }
};


const getUserReactionByPostId = async (postId: string, userId: string, token: string): Promise<ReactionType | null> => {
  try {
    const response = await axios.get<PaginationResponse<ResponseReaction>>(`${BASE_URL}/reactions/post/users/${postId}`, {
      headers: {
        Authorization: `Bearer ${token}`
      },
      params: {
        PageIndex: 1,
        PageSize: 10
      }
    });

    const userReaction = response.data.items.find(reaction => reaction.userId === userId);
    return userReaction ? userReaction.reactionType : null;
  } catch (error) {
    console.error('Failed to fetch user reaction:', error);
    throw error;
  }
};

const updateReaction = async (reactionId: string, reactionType: number, token: string) => {
  try {
    const response = await axios.put(
      `${BASE_URL}/reactions/${reactionId}`,
      { reactionType },
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
    return response.data;
  } catch (error) {
    console.error('Failed to update reaction:', error);
    throw error;
  }
};

const deleteReaction = async (reactionId: string, token: string) => {
  try {
    const response = await axios.delete(`${BASE_URL}/reactions/${reactionId}`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
    return response.data;
  } catch (error) {
    console.error('Failed to delete reaction:', error);
    throw error;
  }
};

export { getReactionsCountByPostId, addReaction, getPostReactions, getUserReactionByPostId, updateReaction, deleteReaction }