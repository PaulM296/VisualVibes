using VisualVibes.App;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;
using Microsoft.EntityFrameworkCore;

namespace VisualVibes.Infrastructure.Repositories
{
    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
        public MessageRepository(VisualVibesDbContext context) : base(context)
        {

        }

        public async Task<ICollection<Message>> GetAllAsync(Guid conversationId)
        {
            var messagesForConversation = await _context.Messages.Where(c => c.ConversationId == conversationId).ToListAsync();

            return messagesForConversation;
        }
    }
}
