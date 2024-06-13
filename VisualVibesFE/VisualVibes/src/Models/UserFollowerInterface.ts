export interface UserFollowerInterface {
    followerId: string;
    followingId: string;
    userName: string;
    firstName: string;
    lastName: string;
    imageId: string;
  }

  export interface UserFollowModalInterface {
    userId: string;
    type: 'followers' | 'following';
    open: boolean;
    onClose: () => void;
  }