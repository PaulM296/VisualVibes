export interface ResponseComment {
    id: string;
    userId: string;
    postId: string;
    text: string;
    createdAt: Date;
    userName: string;
    imageId: string;
    avatar: string;
    commentText: string;
    isModerated: boolean;
}

export interface FormattedComment {
    id: string;
    userId: string;
    userName: string;
    avatar: string;
    text: string;
    createdAt: Date;
    isModerated: boolean;
  }