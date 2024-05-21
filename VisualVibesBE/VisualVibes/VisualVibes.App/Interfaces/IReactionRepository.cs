using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Interfaces
{
    public interface IReactionRepository : IBaseRepository<Reaction>
    {
        Task<ICollection<Reaction>> GetAllAsync(Guid PostId);
        Task<int> GetPostTotalReactionNumber(Guid postId);
    }
}
