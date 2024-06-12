import { useState, useEffect } from 'react';
import { User } from '../Models/User';
import { getUserById, getImageById as getUserImageById } from '../Services/UserServiceApi';
import { getPostsByUserId, getImageById as getPostImageById } from '../Services/UserPostServiceApi';
import { PaginationRequestDto, PaginationResponse } from '../Models/PaginationResponse';
import { ResponsePostModel } from '../Models/ReponsePostModel';

export const useUserProfile = (userId: string) => {
    const [user, setUser] = useState<User | null>(null);
    const [posts, setPosts] = useState<ResponsePostModel[]>([]);
    const [postImages, setPostImages] = useState<{ [key: string]: string }>({});
    const [profilePicture, setProfilePicture] = useState<string>('defaultProfilePicture.jpg');
    const [loading, setLoading] = useState(true);
    const [pageIndex, setPageIndex] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
  
    useEffect(() => {
      const fetchUserData = async () => {
        try {
          const token = localStorage.getItem('token');
          if (!token) {
            console.error('Token not found in localStorage');
            setLoading(false);
            return;
          }
  
          const userData = await getUserById(userId, token);
          setUser(userData);
  
          if (userData.imageId) {
            const imageSrc = await getUserImageById(userData.imageId, token);
            setProfilePicture(imageSrc);
          }
  
          await fetchPosts(userId, 1, token);
        } catch (error) {
          console.error('Error fetching user data:', error);
        } finally {
          setLoading(false);
        }
      };
  
      fetchUserData();
    }, [userId]);
  
    const fetchPosts = async (userId: string, pageIndex: number, token: string) => {
      const paginationRequest: PaginationRequestDto = {
        pageIndex: pageIndex,
        pageSize: 10
      };
  
      const userPostsResponse: PaginationResponse<ResponsePostModel> = await getPostsByUserId(userId, paginationRequest, token);
      const userPosts = userPostsResponse.items;
      setPosts((prevPosts) => {
        const newPosts = userPosts.filter((post: ResponsePostModel) => !prevPosts.some(prevPost => prevPost.id === post.id));
        return [...prevPosts, ...newPosts];
      });
      setPageIndex(userPostsResponse.pageIndex);
      setTotalPages(userPostsResponse.totalPages);
  
      const imagesPromises = userPosts.map(async (post: ResponsePostModel) => {
        if (post.imageId) {
          try {
            const imageSrc = await getPostImageById(post.imageId, token);
            return { postId: post.id, imageSrc };
          } catch (error) {
            console.error(`Failed to fetch image for post ${post.id}:`, error);
            return { postId: post.id, imageSrc: '' };
          }
        }
        return { postId: post.id, imageSrc: '' };
      });
  
      const images = await Promise.all(imagesPromises);
      const imagesMap = images.reduce((acc: { [key: string]: string }, { postId, imageSrc }: { postId: string, imageSrc: string }) => {
        if (imageSrc) {
          acc[postId] = imageSrc;
        }
        return acc;
      }, {} as { [key: string]: string });
  
      setPostImages((prevImages) => ({ ...prevImages, ...imagesMap }));
    };
  
    const loadMorePosts = async () => {
      try {
        const token = localStorage.getItem('token');
        if (!token || !user) return;
        await fetchPosts(user.id, pageIndex + 1, token);
      } catch (error) {
        console.error('Error loading more posts:', error);
      }
    };
  
    return { user, posts, postImages, profilePicture, loading, pageIndex, totalPages, fetchPosts, loadMorePosts };
  };
