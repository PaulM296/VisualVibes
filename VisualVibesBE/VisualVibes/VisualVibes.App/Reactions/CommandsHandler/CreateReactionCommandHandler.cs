using MediatR;
using VisualVibes.App.DTOs.ReactionDtos;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Reactions.Commands;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Reactions.CommandsHandler
{
    public class CreateReactionCommandHandler : IRequestHandler<CreateReactionCommand, ResponseReactionDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateReactionCommandHandler(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseReactionDto> Handle(CreateReactionCommand request, CancellationToken cancellationToken)
        {
            var reaction = new Reaction()
            {
                UserId = request.createReactionDto.UserId,
                PostId = request.createReactionDto.PostId,
                ReactionType = request.createReactionDto.ReactionType,
                Timestamp = DateTime.UtcNow,
            };

            var createdReaction = await _unitOfWork.ReactionRepository.AddAsync(reaction);
            await _unitOfWork.SaveAsync();

            return ResponseReactionDto.FromReaction(createdReaction);
        }
    }
}
