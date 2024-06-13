import React from 'react';
import './Feed.css';
import HomeFeed from '../HomeFeed/HomeFeed';

const Feed: React.FC = () => {
  return (
    <div className="feed">
      <div className="feedWrapper">
        <HomeFeed />
      </div>
    </div>
  );
};

export default Feed;