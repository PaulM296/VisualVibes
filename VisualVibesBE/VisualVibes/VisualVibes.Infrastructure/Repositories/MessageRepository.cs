using VisualVibes.App;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;
using Microsoft.EntityFrameworkCore;

namespace VisualVibes.Infrastructure.Repositories
{
    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
        public MessageRepository(VisualVibesDbContext context, FileSystemLogger logger) : base(context, logger)
        {

        }

        public async Task<ICollection<Message>> GetAllAsync(Guid conversationId)
        {
            var messagesForConversation = await _context.Messages.Where(c => c.ConversationId == conversationId).ToListAsync();

            if (messagesForConversation.Count == 0)
            {
                await _logger.LogAsync(nameof(GetAllAsync), isSuccess: false);
            }
            else
            {
                await _logger.LogAsync(nameof(GetAllAsync), isSuccess: true);
            }

            return messagesForConversation;
        }
    }
}
