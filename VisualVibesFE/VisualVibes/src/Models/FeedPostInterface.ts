import { ResponseComment } from "./ResponseComment";
import { ResponseReaction } from "./ResponseReaction";

export interface FeedPost {
    postId: string;
    userId: string;
    userName: string;
    caption: string | null;
    pictures: string | null;
    createdAt: string;
    reactionCount: number;
    commentCount: number;
    postImageId: string;
    userProfileImageId: string;
    comments: ResponseComment[];
    reactions: ResponseReaction[];
  }
  
  export interface UserFeed {
    userID: string;
    posts: FeedPost[];
  }