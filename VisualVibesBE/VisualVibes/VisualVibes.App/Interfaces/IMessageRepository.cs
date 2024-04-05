using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Interfaces
{
    public interface IMessageRepository : IBaseRepository<Message>
    {
        Task<ICollection<Message>> GetAllAsync(Guid ConversationId);
    }
}
