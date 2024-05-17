using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Interfaces
{
    public interface IFeedRepository : IBaseRepository<Feed>
    {
        Task<Feed> GetByUserIdAsync(string userId);
    }
}
