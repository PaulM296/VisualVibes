using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Interfaces
{
    public interface IPostRepository : IBaseRepository<Post>
    {
        Task<List<Post>> GetTopReactedPostsAsync(int count);
        Task<List<Post>> GetPostsByUserIdAsync(Guid userId);
    }
}
