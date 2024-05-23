﻿using VisualVibes.Domain.Models;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Interfaces
{
    public interface IFeedPostRepository
    {
        Task AddPostToFeedAsync(Guid postId);
        Task<IEnumerable<Post>> GetFeedPostsAsync(Guid feedId);
        Task EnsureFeedForUserAsync(string userId);
        Task<IEnumerable<FeedPost>> GetByFeedIdAsync(Guid feedId);
        Task RemoveAsync(FeedPost feedPost);
        Task<IEnumerable<FeedPost>> GetByPostIdAsync(Guid postId);
        Task<bool> ExistsAsync(Guid feedId, Guid postId);
        Task AddAsync(FeedPost feedPost);
        Task RemoveByPostAndFeedIdAsync(Guid feedId, Guid postId);
    }
}
