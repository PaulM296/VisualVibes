export const getReactionEmoji = (reactionType: string): string => {
    switch (reactionType) {
      case 'Like':
        return '👍';
      case 'Love':
        return '❤️';
      case 'Laugh':
        return '😂';
      case 'Cry':
        return '😢';
      case 'Anger':
        return '😠';
      default:
        return '👍';
    }
  };
  