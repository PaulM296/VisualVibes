using MediatR;
using VisualVibes.App.Comments.Commands;
using VisualVibes.App.DTOs.ReactionDtos;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Reactions.Commands;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Reactions.CommandsHandler
{
    public class UpdateReactionCommandHandler : IRequestHandler<UpdateReactionCommand, ResponseReactionDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateReactionCommandHandler(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseReactionDto> Handle(UpdateReactionCommand request, CancellationToken cancellationToken)
        {
            var getReaction = await _unitOfWork.ReactionRepository.GetByIdAsync(request.id);

            if(getReaction == null)
            {
                throw new ReactionNotFoundException($"The reaction with ID {request.id} doesn't exist and it could not be updated!");
            }

            getReaction.ReactionType = request.updateReactionDto.ReactionType;

            var createdReaction = await _unitOfWork.ReactionRepository.UpdateAsync(getReaction);
            await _unitOfWork.SaveAsync();

            return ResponseReactionDto.FromReaction(createdReaction);
        }
    }
}
