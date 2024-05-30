import React, { useState } from 'react'
import "./ActivityPost.css"
import AccountCircleRoundedIcon from '@mui/icons-material/AccountCircleRounded';
import PermMediaRoundedIcon from '@mui/icons-material/PermMediaRounded';


const ActivityPost = () => {

const [imageLoaded, setImageLoaded] = useState(true);

const handleImageError = () => {
    setImageLoaded(false);
};

  return (
    <div className="activityPost">
        <div className="activityPostWrapper">
            <div className="activityPostTop">
                {imageLoaded && (
                    <img className="activityPostProfileImg" 
                    src="src/assets/testProfilePicture.jpg" 
                    alt="Profile" 
                    onError={handleImageError}
                    />
                )} 
                {!imageLoaded && (
                    <AccountCircleRoundedIcon className="activityPostProfileIcon" />
                )}
                <input 
                    placeholder="What's on your mind?"
                    className="activityPostInput"
                />
            </div>
            <hr className="activityPostHr" />
            <div className="activityPostBottom">
                <div className="activityPostOptions">
                    <div className="activityPostOption">
                        <PermMediaRoundedIcon className="activityPostIcon"/>
                        <span className="activityPostOptionText">Photo</span>
                    </div>
                </div>
                <button className="activityPostButton">Submit</button>
            </div>
        </div>  
    </div>
  )
}

export default ActivityPost;