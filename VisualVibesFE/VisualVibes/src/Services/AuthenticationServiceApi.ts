import apiClient from '../Config/AxiosInterceptor';
import { UserLoginModel } from "../Models/UserLoginModel";

const registerUser = async (userData: FormData) => {
    return await apiClient.post('/users/register', userData, {
        headers: {
            'Content-Type': 'multipart/form-data'
        }
    });
};

const loginUser = async (userData: UserLoginModel) => {
    const response = await apiClient.post('/users/login', userData);
    localStorage.setItem('token', response.data.token);
    return response;
};

export { registerUser, loginUser };
