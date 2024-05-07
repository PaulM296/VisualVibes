using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Interfaces
{
    public interface IFeedPostRepository
    {
        Task AddPostToFeedAsync(Guid postId);
        Task<IEnumerable<Post>> GetFeedPostsAsync(Guid feedId);
        Task EnsureFeedForUserAsync(Guid userId);
    }
}
