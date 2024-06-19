import React from 'react';
import { getReactionEmoji } from '../Utils/getReactionEmoji';
import { reactionTypes } from '../Utils/const/reactionTypes';

interface ReactionButtonProps {
  postId: string;
  userReactions: { [key: string]: string };
  reactionsCount: { [key: string]: number };
  handleReaction: (postId: string, reactionType: string) => void;
  fetchReactions: (postId: string, pageIndex: number) => void;
  showReactions: { [key: string]: boolean };
  setShowReactions: React.Dispatch<React.SetStateAction<{ [key: string]: boolean }>>;
}

const ReactionButton: React.FC<ReactionButtonProps> = ({
  postId, userReactions, reactionsCount, handleReaction, fetchReactions, showReactions, setShowReactions
}) => {
  return (
    <div className="reactionButton"
      onMouseEnter={() => setShowReactions(prev => ({ ...prev, [postId]: true }))}
      onMouseLeave={() => setShowReactions(prev => ({ ...prev, [postId]: false }))}>
      {userReactions[postId] ? (
        <span className="reactionOpener selected" role="img" aria-label={userReactions[postId]}>
          {getReactionEmoji(userReactions[postId])}
        </span>
      ) : (
        <span className="reactionOpener" role="img" aria-label="thumbs up">👍</span>
      )}
      {showReactions[postId] && (
        <div className="reactionOptions">
          {Object.keys(reactionTypes).map(type => (
            <span key={type}
                  className={userReactions[postId] === type ? 'selected' : ''}
                  role="img"
                  aria-label={type}
                  onClick={() => handleReaction(postId, type)}>
              {getReactionEmoji(type)}
            </span>
          ))}
        </div>
      )}
      <span className="feedPostReactionCounter" onClick={() => fetchReactions(postId, 1)}>
        {reactionsCount[postId] || 0} people reacted
      </span>
    </div>
  );
};

export default ReactionButton;
