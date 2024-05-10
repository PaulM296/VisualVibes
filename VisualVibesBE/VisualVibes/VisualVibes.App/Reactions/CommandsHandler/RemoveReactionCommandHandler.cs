using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Reactions.Commands;

namespace VisualVibes.App.Reactions.CommandsHandler
{
    public class RemoveReactionCommandHandler : IRequestHandler<RemoveReactionCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RemoveReactionCommandHandler> _logger;

        public RemoveReactionCommandHandler(IUnitOfWork unitOfWork, ILogger<RemoveReactionCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<Unit> Handle(RemoveReactionCommand request, CancellationToken cancellationToken)
        {
            var reactionToRemove = await _unitOfWork.ReactionRepository.GetByIdAsync(request.Id);

            if(reactionToRemove == null)
            {
                throw new ReactionNotFoundException($"The reaction with ID {request.Id} doesn't exist and it could not be removed!");
            };

            await _unitOfWork.ReactionRepository.RemoveAsync(reactionToRemove);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Reaction successfully removed!");

            return Unit.Value;
        }
    }
}
