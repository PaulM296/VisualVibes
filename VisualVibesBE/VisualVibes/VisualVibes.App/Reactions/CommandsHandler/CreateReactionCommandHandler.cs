using MediatR;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Reactions.Commands;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Reactions.CommandsHandler
{
    public class CreateReactionCommandHandler : IRequestHandler<CreateReactionCommand, ReactionDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateReactionCommandHandler(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ReactionDto> Handle(CreateReactionCommand request, CancellationToken cancellationToken)
        {
            var reaction = new Reaction()
            {
                Id = request.ReactionDto.Id,
                UserId = request.ReactionDto.UserId,
                PostId = request.ReactionDto.PostId,
                ReactionType = request.ReactionDto.ReactionType,
                Timestamp = request.ReactionDto.Timestamp
            };

            var createdReaction = await _unitOfWork.ReactionRepository.AddAsync(reaction);
            await _unitOfWork.SaveAsync();

            return ReactionDto.FromReaction(createdReaction);
        }
    }
}
