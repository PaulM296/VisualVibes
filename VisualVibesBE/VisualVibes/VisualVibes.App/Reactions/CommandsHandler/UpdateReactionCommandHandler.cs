using MediatR;
using VisualVibes.App.Comments.Commands;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Reactions.Commands;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Reactions.CommandsHandler
{
    public class UpdateReactionCommandHandler : IRequestHandler<UpdateReactionCommand, ReactionDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateReactionCommandHandler(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ReactionDto> Handle(UpdateReactionCommand request, CancellationToken cancellationToken)
        {
            var getReaction = await _unitOfWork.ReactionRepository.GetByIdAsync(request.ReactionDto.Id);

            getReaction.ReactionType = request.ReactionDto.ReactionType;
            getReaction.Timestamp = request.ReactionDto.Timestamp;

            var createdReaction = await _unitOfWork.ReactionRepository.UpdateAsync(getReaction);
            await _unitOfWork.SaveAsync();

            return ReactionDto.FromReaction(createdReaction);
        }
    }
}
