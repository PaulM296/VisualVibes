using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.Conversations.Commands;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;

namespace VisualVibes.App.Conversations.CommandsHandler
{
    public class RemoveConversationCommandHandler : IRequestHandler<RemoveConversationCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RemoveConversationCommandHandler> _logger;

        public RemoveConversationCommandHandler(IUnitOfWork unitOfWork, ILogger<RemoveConversationCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<Unit> Handle(RemoveConversationCommand request, CancellationToken cancellationToken)
        {
            var conversationToRemove = await _unitOfWork.ConversationRepository.GetByIdAsync(request.Id);

            if(conversationToRemove == null)
            {
                throw new ConversationNotFoundException($"The conversation with ID {request.Id} doesn't exist and it could not be removed!");
            }

            await _unitOfWork.ConversationRepository.RemoveAsync(conversationToRemove);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Conversation successfully removed!");

            return Unit.Value;
        }
    }
}
