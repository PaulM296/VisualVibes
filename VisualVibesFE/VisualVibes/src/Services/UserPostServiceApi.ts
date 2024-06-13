import apiClient from '../Config/AxiosInterceptor';
import { CreatePostModel } from "../Models/CreatePostModel";
import { ResponsePostModel } from "../Models/ReponsePostModel";
import { PaginationRequestDto, PaginationResponse } from "../Models/PaginationResponse";

const createPost = async (createPostDto: CreatePostModel): Promise<ResponsePostModel> => {
    const formData = new FormData();
    formData.append('caption', createPostDto.caption);
    if (createPostDto.image) {
      formData.append('image', createPostDto.image);
    }
  
    const response = await apiClient.post<ResponsePostModel>('/activityPosts', formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      }
    });
  
    const responseData: ResponsePostModel = {
      ...response.data,
      createdAt: new Date(response.data.createdAt),
    };
  
    return responseData;
  };

const getImageById = async (imageId: string): Promise<string> => {
  const response = await apiClient.get<{ imageSrc: string }>(`/image/${imageId}`);
  return response.data.imageSrc;
};

const getPostsByUserId = async (userId: string, paginationRequest: PaginationRequestDto): Promise<PaginationResponse<ResponsePostModel>> => {
  const response = await apiClient.get<PaginationResponse<ResponsePostModel>>(`/activityPosts/user/${userId}`, {
      params: {
          pageIndex: paginationRequest.pageIndex,
          pageSize: paginationRequest.pageSize,
      }
  });

  const posts = response.data.items.map(post => ({
      ...post,
      createdAt: new Date(post.createdAt),
      comments: post.comments.map(comment => ({
          ...comment,
          createdAt: new Date(comment.createdAt),
      })),
      reactions: post.reactions.map(reaction => ({
          ...reaction,
          timestamp: new Date(reaction.timestamp).toISOString(),
      }))
  }));

  return {
      ...response.data,
      items: posts,
  };
};
  
export { createPost, getPostsByUserId, getImageById };
