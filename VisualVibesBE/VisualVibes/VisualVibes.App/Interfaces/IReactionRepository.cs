using VisualVibes.App.DTOs.PaginationDtos;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Interfaces
{
    public interface IReactionRepository : IBaseRepository<Reaction>
    {
        Task<ICollection<Reaction>> GetAllAsync(Guid PostId);
        Task<int> GetPostTotalReactionNumber(Guid postId);
        Task<PaginationResponseDto<Reaction>> GetAllPagedReactionsAsync(Guid postId, int pageIndex, int pageSize);
    }
}
