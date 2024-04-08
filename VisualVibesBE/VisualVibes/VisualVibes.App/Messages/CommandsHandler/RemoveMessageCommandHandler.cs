using MediatR;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Messages.Commands;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Messages.CommandsHandler
{
    public class RemoveMessageCommandHandler : IRequestHandler<RemoveMessagedCommand, Unit>
    {
        private readonly IMessageRepository _messageRepository;

        public RemoveMessageCommandHandler(IMessageRepository messageRepository) 
        {
            _messageRepository = messageRepository;
        }
        public async Task<Unit> Handle(RemoveMessagedCommand request, CancellationToken cancellationToken)
        {
            var messageToRemove = await _messageRepository.GetByIdAsync(request.Id);
            if (messageToRemove == null)
            {
                throw new Exception($"User with ID {request.Id} not found.");
            };

            await _messageRepository.RemoveAsync(messageToRemove);
            return Unit.Value;

        }
    }
}
