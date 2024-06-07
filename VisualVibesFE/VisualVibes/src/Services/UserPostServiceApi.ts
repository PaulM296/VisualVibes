import axios from "axios";
import { CreatePostModel } from "../Models/CreatePostModel";
import { BASE_URL } from "../Config/ApiConfig";
import { ResponsePostModel } from "../Models/ReponsePostModel";

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
  

export default { createPost };