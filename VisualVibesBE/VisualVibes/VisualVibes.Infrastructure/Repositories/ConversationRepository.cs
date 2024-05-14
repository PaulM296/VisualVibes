using VisualVibes.App;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;
using Microsoft.EntityFrameworkCore;


namespace VisualVibes.Infrastructure.Repositories
{
    public class ConversationRepository : BaseRepository<Conversation>, IConversationRepository
    {
        public ConversationRepository(VisualVibesDbContext context) : base(context)
        {

        }

        public async Task<List<Conversation>> GetAllByUserIdAsync(Guid userId)
        {
            return await _context.Conversations
            .Where(c => c.FirstParticipantId == userId || c.SecondParticipantId == userId)
            .ToListAsync();
        }
    }
}
