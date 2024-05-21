using VisualVibes.App;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;
using Microsoft.EntityFrameworkCore;
using VisualVibes.App.DTOs.PaginationDtos;

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

        public async Task<PaginationResponseDto<Message>> GetAllPagedConversationMessagesAsync(Guid conversationId, int pageIndex, int pageSize)
        {
            var messagesForConversation = await _context.Messages
                .Where(m => m.ConversationId == conversationId)
                .OrderByDescending(m => m.Timestamp)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();


            var count = await _context.Messages.Where(m => m.ConversationId == conversationId).CountAsync();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);


            return new PaginationResponseDto<Message>(messagesForConversation, pageIndex, totalPages);
        }
    }
}
