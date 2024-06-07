import axios from "axios";
import { BASE_URL } from "../Config/ApiConfig";
import { User } from "../Models/User";

const getUserById = async (userId: string, token: string): Promise<User> => {
    const response = await axios.get<User>(`${BASE_URL}/users/${userId}`, {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    });
    return response.data;
};

const getImageById = async (imageId: string, token: string): Promise<string> => {
    try {
        const response = await axios.get<{ imageSrc: string }>(`${BASE_URL}/image/${imageId}`, {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });
        return response.data.imageSrc;
    } catch (error) {
        console.error(`Error fetching image with ID ${imageId}:`, error);
        throw error;
    }
};

export { getUserById, getImageById };