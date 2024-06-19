import { ResponseComment } from './ResponseComment';
import { ResponseReaction } from './ResponseReaction';

export interface ResponsePostModel {
  id: string;
  userId: string;
  userName: string;
  caption: string;
  pictures: string;
  createdAt: Date;
  imageId: string;
  comments: ResponseComment[];
  reactions: ResponseReaction[];
  isModerated: boolean;
}

export interface UpdatePostModel {
  caption: string;
}