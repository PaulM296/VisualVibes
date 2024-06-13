import { Modal, Typography, List, ListItem, ListItemAvatar, Avatar, ListItemText } from "@mui/material";
import { useState, useEffect } from "react";
import { UserFollowModalInterface, UserFollowerInterface } from "../../Models/UserFollowerInterface";
import { getUserFollowing, getUserFollowers, getImageById } from "../../Services/UserServiceApi";
import { User } from "../../Models/User";

const UserFollowModal: React.FC<UserFollowModalInterface> = ({ userId, type, open, onClose }) => {
    const [users, setUsers] = useState<UserFollowerInterface[]>([]);
    const [loading, setLoading] = useState<boolean>(false);
    const [avatars, setAvatars] = useState<{ [key: string]: string }>({});

    useEffect(() => {
        const fetchData = async () => {
            const token = localStorage.getItem('token');
            if (userId && token) {
                try {
                    setLoading(true);
                    let userList: User[] | UserFollowerInterface[] = [];

                    if (type === 'following') {
                        userList = await getUserFollowing(userId);
                    } else {
                        const followersList = await getUserFollowers(userId);
                        userList = followersList.map((user: User) => ({
                            followerId: user.id,
                            followingId: user.id,
                            userName: user.userName,
                            firstName: user.firstName,
                            lastName: user.lastName,
                            imageId: user.imageId
                        }));
                    }

                    setUsers(userList);

                    const avatarPromises = userList.map(user =>
                        getImageById(user.imageId).then(imageSrc => ({ [user.imageId]: imageSrc }))
                    );
                    const avatarResults = await Promise.all(avatarPromises);
                    const avatarMap = avatarResults.reduce((acc, curr) => ({ ...acc, ...curr }), {});
                    setAvatars(avatarMap);
                } catch (error) {
                    console.error(`Error fetching ${type} users:`, error);
                } finally {
                    setLoading(false);
                }
            }
        };

        if (open) {
            fetchData();
        } else {
            setUsers([]);
            setAvatars({});
            setLoading(false);
        }
    }, [userId, type, open]);

    return (
        <Modal open={open} onClose={onClose}>
            <div className="modalContent">
                <Typography variant="h6">{type.charAt(0).toUpperCase() + type.slice(1)}</Typography>
                {loading ? (
                    <p>Loading...</p>
                ) : users.length === 0 ? (
                    <p>No users found.</p>
                ) : (
                    <List>
                        {users.map((user) => (
                            <ListItem key={user.followingId} button>
                                <ListItemAvatar>
                                    <Avatar src={avatars[user.imageId] || ''} alt={user.userName} />
                                </ListItemAvatar>
                                <ListItemText primary={user.userName} secondary={`${user.firstName} ${user.lastName}`} />
                            </ListItem>
                        ))}
                    </List>
                )}
            </div>
        </Modal>
    );
};

export default UserFollowModal;
