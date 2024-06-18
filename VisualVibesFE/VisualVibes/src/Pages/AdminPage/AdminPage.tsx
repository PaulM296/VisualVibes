import React, { useEffect, useState } from 'react';
import { useUser } from '../../Hooks/userContext';
import { getPaginatedUsers, blockUser, unblockUser, getImageById } from '../../Services/UserServiceApi';
import UserList from '../../Components/UserList/UserList';
import Pagination from '../../Components/Pagination/Pagination';
import { User } from '../../Models/User';
import './AdminPage.css';

const AdminPage: React.FC = () => {
  const { isAdmin } = useUser();
  const [users, setUsers] = useState<User[]>([]);
  const [userImages, setUserImages] = useState<{ [key: string]: string }>({});
  const [pageIndex, setPageIndex] = useState(1);
  const [totalPages, setTotalPages] = useState(1);

  useEffect(() => {
    if (isAdmin) {
      fetchUsers(pageIndex);
    }
  }, [isAdmin, pageIndex]);

  const fetchUsers = async (page: number) => {
    try {
      const paginationRequest = { pageIndex: page, pageSize: 5 };
      const response = await getPaginatedUsers(paginationRequest);
      setUsers(response.items);
      setTotalPages(response.totalPages);

      const imagePromises = response.items.map(async (user) => {
        const imageSrc = user.imageId ? await getImageById(user.imageId) : '';
        return { userId: user.id, imageSrc };
      });

      const images = await Promise.all(imagePromises);
      const imagesMap = images.reduce((acc, { userId, imageSrc }) => {
        acc[userId] = imageSrc;
        return acc;
      }, {} as { [key: string]: string });

      setUserImages(imagesMap);
    } catch (error) {
      console.log(error);
    }
  };

  const handleBlockUser = async (userId: string) => {
    await blockUser(userId);
    fetchUsers(pageIndex);
  };

  const handleUnblockUser = async (userId: string) => {
    await unblockUser(userId);
    fetchUsers(pageIndex);
  };

  return (
    <div className="admin-page">
      <h1>VisualVibes Users</h1>
      <UserList users={users} userImages={userImages} onBlock={handleBlockUser} onUnblock={handleUnblockUser} />
      <Pagination pageIndex={pageIndex} totalPages={totalPages} setPageIndex={setPageIndex} />
    </div>
  );
};

export default AdminPage;
