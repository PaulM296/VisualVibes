export interface UserRegistrationModel {
    firstName: string;
    lastName: string;
    email: string;
    username: string;
    password: string;
    confirmPassword: string;
    dateOfBirth: Date;
    role: 'admin' | 'user';
    bio?: string;
    image?: FileList | null;
}