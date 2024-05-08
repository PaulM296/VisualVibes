using VisualVibes.Domain.Models;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Interfaces
{
    public interface IFeedPostRepository
    {
        Task AddPostToFeedAsync(Guid postId);
        Task<IEnumerable<Post>> GetFeedPostsAsync(Guid feedId);
        Task EnsureFeedForUserAsync(Guid userId);
        Task<IEnumerable<FeedPost>> GetByFeedIdAsync(Guid feedId);
        Task RemoveAsync(FeedPost feedPost);
    }
}
