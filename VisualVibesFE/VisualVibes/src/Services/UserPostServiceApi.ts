import axios from "axios";
import { CreatePostModel } from "../Models/CreatePostModel";
import { BASE_URL } from "../Config/ApiConfig";
import { ResponsePostModel } from "../Models/ReponsePostModel";
import { PaginationRequestDto, PaginationResponse } from "../Models/PaginationResponse";

const createPost = async (createPostDto: CreatePostModel, token: string): Promise<ResponsePostModel> => {
    const formData = new FormData();
    formData.append('caption', createPostDto.caption);
    if (createPostDto.image) {
      formData.append('image', createPostDto.image);
    }
  
    const response = await axios.post<ResponsePostModel>(`${BASE_URL}/activityPosts`, formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
        'Authorization': `Bearer ${token}`
      }
    });
  
    const responseData: ResponsePostModel = {
      ...response.data,
      createdAt: new Date(response.data.createdAt),
    };
  
    return responseData;
  };

//   const getPostsByUserId = async (userId: string, token: string): Promise<ResponsePostModel[]> => {
//     const response = await axios.get<ResponsePostModel[]>(`${BASE_URL}/activityPosts/user/${userId}`, {
//         headers: {
//             'Authorization': `Bearer ${token}`
//         }
//     });
//     return response.data;
// };

const getImageById = async (imageId: string, token: string): Promise<string> => {
  const response = await axios.get<{ imageSrc: string }>(`${BASE_URL}/image/${imageId}`, {
      headers: {
          'Authorization': `Bearer ${token}`
      }
  });
  return response.data.imageSrc;
};

const getPostsByUserId = async (userId: string, paginationRequest: PaginationRequestDto, token: string): Promise<PaginationResponse<ResponsePostModel>> => {
  const response = await axios.get<PaginationResponse<ResponsePostModel>>(`${BASE_URL}/activityPosts/user/${userId}`, {
      headers: {
          'Authorization': `Bearer ${token}`
      },
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