﻿using VisualVibes.App;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Infrastructure.Repositories
{
    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
        private static IList<Message> _messages = new List<Message>();
        private readonly FileSystemLogger _logger;
        public MessageRepository(FileSystemLogger logger) : base(logger)
        {

        }

        public async Task<ICollection<Message>> GetAllAsync(Guid conversationId)
        {
            var messagesForConversation = _messages.Where(c => c.ConversationId == conversationId).ToList();

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
