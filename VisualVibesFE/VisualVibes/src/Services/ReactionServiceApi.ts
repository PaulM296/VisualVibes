import axios from 'axios';
import { BASE_URL } from '../Config/ApiConfig';
import { ReactionType } from '../Models/ReactionType';
import { PaginationResponse } from '../Models/PaginationResponse';
import { ResponseReaction } from '../Models/ResponseReaction';

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

export const addReaction = async (postId: string, reactionType: number, token: string) => {
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

export const getPostReactions = async (postId: string, token: string, pageIndex: number = 1, pageSize: number = 10): Promise<PaginationResponse<ResponseReaction>> => {
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


export const getUserReactionByPostId = async (postId: string, userId: string, token: string): Promise<ReactionType | null> => {
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

