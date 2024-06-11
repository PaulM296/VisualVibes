import React, { useState } from 'react';
import { Avatar } from '@mui/material';
import { searchUsers, getImageById } from '../../Services/UserServiceApi';
import { User } from '../../Models/User';

const UserSearch: React.FC = () => {
    const [query, setQuery] = useState('');
    const [users, setUsers] = useState<User[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');

    const handleSearch = async () => {
        setLoading(true);
        setError('');
        try {
            const token = localStorage.getItem('token');
            if (!token) {
                throw new Error('User not authenticated');
            }
            const searchResults = await searchUsers(query, token);
            const usersWithImages = await Promise.all(searchResults.map(async (user) => {
                const imageSrc = await getImageById(user.imageId, token);
                return { ...user, profilePicture: imageSrc };
            }));
            setUsers(usersWithImages);
        } catch (err) {
            setError('Error fetching users.');
            console.error(err);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div>
            <input
                type="text"
                value={query}
                onChange={(e) => setQuery(e.target.value)}
                placeholder="Search users..."
            />
            <button onClick={handleSearch} disabled={loading}>
                {loading ? 'Searching...' : 'Search'}
            </button>
            {error && <p>{error}</p>}
            <div>
                {users.map((user) => (
                    <div key={user.id}>
                        <Avatar src={user.profilePicture || undefined} alt={`${user.userName}'s profile`}>
                            {user.userName.charAt(0)}
                        </Avatar>
                        <p>{user.userName} ({user.firstName} {user.lastName})</p>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default UserSearch;
