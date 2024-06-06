import axios from "axios";
import { BASE_URL } from "../Config/ApiConfig";
import { UserLoginModel } from "../Models/UserLoginModel";

const registerUser = (userData: FormData) => {
    return axios.post(`${BASE_URL}/users/register`, userData, {
        headers: {
            'Content-Type': 'multipart/form-data'
        }
    });
};

const loginUser = async (userData: UserLoginModel) => {
    const response = await axios.post(`${BASE_URL}/users/login`, userData);
    localStorage.setItem('token', response.data.token);
    return response;
};

export { registerUser, loginUser };