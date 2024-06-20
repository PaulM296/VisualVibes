import React, { useEffect, useState } from 'react';
import { getAdminPosts, getImageById } from '../../Services/UserPostServiceApi';
import { ResponsePostModel } from '../../Models/ReponsePostModel';
import AdminPostItem from '../AdminPostItem/AdminPostItem';
import './Rightbar.css';

const Rightbar: React.FC = () => {
  const [adminPosts, setAdminPosts] = useState<ResponsePostModel[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [avatars, setAvatars] = useState<{ [key: string]: string }>({});
  const [pageIndex, setPageIndex] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const pageSize = 1;

  useEffect(() => {
    const fetchAdminPosts = async () => {
      try {
        setLoading(true);
        const paginationRequest = { pageIndex, pageSize };
        const response = await getAdminPosts(paginationRequest);
        console.log("Admin Posts:", response.items);
        setAdminPosts(response.items);
        setTotalPages(response.totalPages);

        const avatarPromises = response.items.map(post =>
          getImageById(post.imageId).then(imageSrc => ({ [post.imageId]: imageSrc }))
        );
        const avatarResults = await Promise.all(avatarPromises);
        const avatarMap = avatarResults.reduce((acc, curr) => ({ ...acc, ...curr }), {});
        setAvatars(avatarMap);
      } catch (error) {
        console.error('Error fetching admin posts:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchAdminPosts();
  }, [pageIndex]);

  if (loading) {
    return <p>Loading...</p>;
  }

  return (
    <div className="rightbar">
      <h3>Admin Posts</h3>
      {adminPosts.length === 0 ? (
        <p>No posts by admins.</p>
      ) : (
        adminPosts.map((post) => (
          <AdminPostItem key={post.id} post={post} avatarSrc={avatars[post.imageId] || ''} />
        ))
      )}
      <div className="rightBarPaginationControls">
        <button disabled={pageIndex === 1} onClick={() => setPageIndex(pageIndex - 1)}>
          Previous
        </button>
        <button disabled={pageIndex === totalPages} onClick={() => setPageIndex(pageIndex + 1)}>Next</button>
      </div>
    </div>
  );
};

export default Rightbar;
