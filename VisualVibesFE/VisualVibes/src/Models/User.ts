export interface User {
    id: string;
    userName: string;
    email: string;
    role: number;
    firstName: string;
    lastName: string;
    dateOfBirth: string;
    bio: string;
    profilePicture: string | null;
    imageId: string;
    isBlocked: boolean;
}

export interface UserContextType {
    user: User | undefined;
    isAdmin: boolean;
    setUser: React.Dispatch<React.SetStateAction<User | undefined>>;
}