import { ResponseComment } from './ResponseComment';
import { ResponseReaction } from './ResponseReaction';

export interface ResponsePostModel {
  id: string;
  userId: string;
  caption: string;
  pictures: string;
  createdAt: Date;
  imageId: string;
  comments: ResponseComment[];
  reactions: ResponseReaction[];
}