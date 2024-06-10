import { ReactionType } from "./ReactionType";

export interface ResponseReaction {
    id: string;
    userId: string;
    postId: string;
    reactionType: ReactionType;
    timestamp: string;
}