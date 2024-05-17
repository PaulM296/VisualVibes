using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Interfaces
{
    public interface IConversationRepository : IBaseRepository<Conversation>
    {
        Task<List<Conversation>> GetAllByUserIdAsync(string userId);
    }
}
