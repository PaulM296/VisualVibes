using MediatR;
using VisualVibes.App.Conversations.Commands;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Conversations.CommandsHandler
{
    public class RemoveConversationCommandHandler : IRequestHandler<RemoveConversationCommand, Unit>
    {
        private readonly IConversationRepository _conversationRepository;
        public RemoveConversationCommandHandler(IConversationRepository conversationRepository)
        {
            _conversationRepository = conversationRepository;
        }
        public async Task<Unit> Handle(RemoveConversationCommand request, CancellationToken cancellationToken)
        {
            var conversationToRemove = await _conversationRepository.GetByIdAsync(request.Id);
            if(conversationToRemove == null)
            {
                throw new Exception($"Conversation with ID {request.Id} not found.");
            }
            await _conversationRepository.RemoveAsync(conversationToRemove);
            return Unit.Value;
        }
    }
}
