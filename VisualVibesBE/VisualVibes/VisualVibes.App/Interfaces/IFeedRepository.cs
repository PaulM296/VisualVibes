﻿using VisualVibes.App.DTOs.PaginationDtos;
using VisualVibes.Domain.Models;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Interfaces
{
    public interface IFeedRepository : IBaseRepository<Feed>
    {
        Task<Feed> GetByUserIdAsync(string userId);
        Task<List<FeedPost>> GetFeedPostsByUserIdAsync(string userId);
        Task<PaginationResponseDto<FeedPost>> GetPagedFeedPostsByUserIdAsync(string userId, int pageIndex, int pageSize);
    }
}
