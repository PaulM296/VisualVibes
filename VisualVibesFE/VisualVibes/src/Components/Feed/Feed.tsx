import React from 'react'
import "./Feed.css"
import ActivityPost from '../ActivityPost/ActivityPost'
import FeedPost from '../FeedPost/FeedPost'

const Feed = () => {
  return (
    <div className="feed">
        <div className="feedWrapper">
          <ActivityPost />
          <FeedPost />
          <FeedPost />
          <FeedPost />
          <FeedPost />
          <FeedPost />
        </div>
    </div>
  )
}

export default Feed