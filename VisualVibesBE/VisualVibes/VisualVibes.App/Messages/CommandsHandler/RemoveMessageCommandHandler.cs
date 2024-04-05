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
            var message = new Message()
            {
                Id = request.Id,
            };

            await _messageRepository.RemoveAsync(message);
            return Unit.Value;

        }
    }
}
