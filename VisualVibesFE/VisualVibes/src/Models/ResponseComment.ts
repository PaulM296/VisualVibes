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
}

export interface FormattedComment {
    userName: string;
    avatar: string;
    text: string;
    createdAt: Date;
  }