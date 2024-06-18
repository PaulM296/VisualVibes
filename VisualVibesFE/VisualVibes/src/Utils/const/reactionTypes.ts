import { ReactionType } from "../../Models/ReactionType";

export const reactionTypes: { [key: string]: number } = {
  Like: ReactionType.Like,
  Love: ReactionType.Love,
  Laugh: ReactionType.Laugh,
  Cry: ReactionType.Cry,
  Anger: ReactionType.Anger,
};
