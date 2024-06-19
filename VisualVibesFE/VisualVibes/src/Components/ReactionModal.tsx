import React from 'react';
import { Modal, Typography, Avatar, Button } from '@mui/material';
import { ReactionWithEmoji } from '../Models/ReactionWithEmoji';

interface ReactionModalProps {
  open: boolean;
  onClose: () => void;
  reactions: ReactionWithEmoji[];
  pageIndex: number;
  totalPages: number;
  fetchReactions: (postId: string, pageIndex: number) => void;
  currentPostId: string | null;
}

const ReactionModal: React.FC<ReactionModalProps> = ({ open, onClose, reactions, pageIndex, totalPages, fetchReactions, currentPostId }) => {
  return (
    <Modal open={open} onClose={onClose}>
      <div className="modalContent">
        <Typography variant="h6">Reactions</Typography>
        {reactions.length === 0 && <Typography sx={{ mt: 2 }}>No reactions yet.</Typography>}
        {reactions.length > 0 && reactions.map((reaction, index) => (
          <div key={index} className="reactionItem">
            <span>{reaction.reactionEmoji}</span>
            <Avatar src={reaction.avatar} alt={reaction.userName} sx={{ margin: '0 10px' }} />
            <Typography>{reaction.userName}</Typography>
          </div>
        ))}
        <div className="paginationControls">
          <Button
            disabled={pageIndex === 1}
            onClick={() => fetchReactions(currentPostId!, pageIndex - 1)}
            style={{ float: 'left' }}
          >
            Previous
          </Button>
          <Typography>{pageIndex} / {totalPages}</Typography>
          <Button
            disabled={pageIndex === totalPages}
            onClick={() => fetchReactions(currentPostId!, pageIndex + 1)}
            style={{ float: 'right' }}
          >
            Next
          </Button>
        </div>
      </div>
    </Modal>
  );
};

export default ReactionModal;
