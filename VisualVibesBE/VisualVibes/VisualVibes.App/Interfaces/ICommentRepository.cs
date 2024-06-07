using VisualVibes.App.DTOs.PaginationDtos;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Interfaces
{
    public interface ICommentRepository : IBaseRepository<Comment>
    {
        Task<ICollection<Comment>> GetAllAsync(Guid postId);

        Task<PaginationResponseDto<Comment>> GetAllPagedCommentsAsync(Guid postId, int pageIndex, int pageSize);

        Task<int> GetPostTotalCommentNumber(Guid postId);
    }
}
