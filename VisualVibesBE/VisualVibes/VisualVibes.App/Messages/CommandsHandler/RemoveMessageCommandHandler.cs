using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Messages.Commands;

namespace VisualVibes.App.Messages.CommandsHandler
{
    public class RemoveMessageCommandHandler : IRequestHandler<RemoveMessageCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RemoveMessageCommandHandler> _logger;

        public RemoveMessageCommandHandler(IUnitOfWork unitOfWork, ILogger<RemoveMessageCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<Unit> Handle(RemoveMessageCommand request, CancellationToken cancellationToken)
        {
            var messageToRemove = await _unitOfWork.MessageRepository.GetByIdAsync(request.Id);

            if (messageToRemove == null)
            {
                throw new MessageNotFoundException($"The message with ID {request.Id} doesn't exist and it could not be removed!");
            };

            await _unitOfWork.MessageRepository.RemoveAsync(messageToRemove);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Message successfully removed!");

            return Unit.Value;

        }
    }
}
