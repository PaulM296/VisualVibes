using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Interfaces
{
    public interface ICommentRepository : IBaseRepository<Comment>
    {
        Task<ICollection<Comment>> GetAllAsync(Guid PostId);
    }
}
