using MediatR;
using VisualVibes.App.DTOs;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Reactions.Commands;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Reactions.CommandsHandler
{
    public class RemoveReactionCommandHandler : IRequestHandler<RemoveReactionCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        public RemoveReactionCommandHandler(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
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

            return Unit.Value;
        }
    }
}
