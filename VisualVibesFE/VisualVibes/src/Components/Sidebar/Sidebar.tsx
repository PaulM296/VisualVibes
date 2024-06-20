import React, { useEffect, useState } from "react";
import {
  Avatar,
  List,
  ListItem,
  ListItemAvatar,
  ListItemText,
} from "@mui/material";
import { useNavigate } from "react-router-dom";
import { getUserFollowing, getImageById } from "../../Services/UserServiceApi";
import { getUserIdFromToken } from "../../Utils/auth";
import "./Sidebar.css";
import { UserFollowerInterface } from "../../Models/UserFollowerInterface";

const Sidebar: React.FC = () => {
  const [following, setFollowing] = useState<UserFollowerInterface[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [avatars, setAvatars] = useState<{ [key: string]: string }>({});
  const navigate = useNavigate();

  useEffect(() => {
    const fetchFollowing = async () => {
      const userId = getUserIdFromToken();
      const token = localStorage.getItem("token");
      if (userId && token) {
        try {
          const followingList = await getUserFollowing(userId);
          setFollowing(followingList);

          const avatarPromises = followingList.map((user) =>
            getImageById(user.imageId).then((imageSrc) => ({
              [user.imageId]: imageSrc,
            }))
          );
          const avatarResults = await Promise.all(avatarPromises);
          const avatarMap = avatarResults.reduce(
            (acc, curr) => ({ ...acc, ...curr }),
            {}
          );
          setAvatars(avatarMap);
        } catch (error) {
          console.error("Error fetching following users:", error);
        } finally {
          setLoading(false);
        }
      }
    };

    fetchFollowing();
  }, []);



  const handleUserClick = (userId: string) => {
    navigate(`/user/${userId}`);
  };

  return (
    <div className="sidebar">
      <h3>Following</h3>
      {loading && 'Loading'}
     {!loading &&<> {following.length === 0 && (
        <p className="no-following-message">You are not following anyone.</p>
      )}
      <div className="sidebarList">
        <List>
          {following.map((user) => (
            <ListItem
              key={user.followingId}
              
              onClick={() => handleUserClick(user.followingId)}
            >
              <ListItemAvatar>
                <Avatar  style={{border: '1px solid black'}} src={avatars[user.imageId] || ""} alt={user.userName} />
              </ListItemAvatar>
              <ListItemText
                primary={user.userName}
                secondary={`${user.firstName} ${user.lastName}`}
              />
            </ListItem>
          ))}
        </List>
      </div></>}
    </div>
  );
};

export default Sidebar;
