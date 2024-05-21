using VisualVibes.App.DTOs.PaginationDtos;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Interfaces
{
    public interface IMessageRepository : IBaseRepository<Message>
    {
        Task<ICollection<Message>> GetAllAsync(Guid ConversationId);

        Task<PaginationResponseDto<Message>> GetAllPagedConversationMessagesAsync(Guid conversationId, int pageIndex, int pageSize);
    }
}
