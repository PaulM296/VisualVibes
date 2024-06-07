import React from 'react'
import "./FeedPost.css"
import { MoreVert } from '@mui/icons-material'

const FeedPost = () => {
  return (
    <div className="feedPost">
        <div className="feedPostWrapper">
            <div className="feedPostTop">
                <div className="feedPostTopLeft">
                    <img className="feedPostProfileImg" 
                    src="src/assets/testProfilePicture.jpg" 
                    alt="" />
                    <span className="feedPostUsername"> Paulinho</span>
                    <span className="feedPostDate">5 mins ago</span>
                </div>
                <div className="feedPostTopRight">
                    <MoreVert />
                </div>
            </div>
            <div className="feedPostCenter">
                <span className="feedPostText"> Hey! This is my first post!</span>
                <img className="feedPostImg" 
                src="src/assets/forestTestImage.png"
                alt="" />
            </div>
            <div className="feedPostBottom">
                <div className="feedPostBottomLeft">
                    <img className="reactionIcon" src="src/assets/Like.png"/>
                    <img className="reactionIcon" src="src/assets/Love.png"/>
                    <span className="feedPostReactionCounter">15 people like this</span>
                </div>
                <div className="feedPostBottomRight">
                    <span className="feedPostCommentText"> 7 comments</span>
                </div>
            </div>
        </div>
    </div>
  )
}

export default FeedPost;