import axios from "axios";
import { BASE_URL } from "../Config/ApiConfig";
import { UserLoginModel } from "../Models/UserLoginModel";

const registerUser = (userData: FormData) => {
    return axios.post(`${BASE_URL}/register`, userData, {
        headers: {
            'Content-Type': 'multipart/form-data'
        }
    });
};

const loginUser = (userData: UserLoginModel) => {
    return axios.post(`${BASE_URL}/login`, userData);
};

export { registerUser, loginUser };