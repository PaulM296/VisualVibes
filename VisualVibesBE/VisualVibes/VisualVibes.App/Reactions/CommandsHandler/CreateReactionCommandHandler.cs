using MediatR;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Reactions.Commands;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Reactions.CommandsHandler
{
    public class CreateReactionCommandHandler : IRequestHandler<CreateReactionCommand, ReactionDto>
    {
        private readonly IReactionRepository _reactionRepository;
        public CreateReactionCommandHandler(IReactionRepository reactionRepository) 
        {
            _reactionRepository = reactionRepository;
        }
        public async Task<ReactionDto> Handle(CreateReactionCommand request, CancellationToken cancellationToken)
        {
            var reaction = new Reaction()
            {
                Id = Guid.NewGuid(),
                UserId = request.ReactionDto.UserId,
                PostId = request.ReactionDto.PostId,
                ReactionType = request.ReactionDto.ReactionType,
                Timestamp = request.ReactionDto.Timestamp
            };
            var createdReaction = await _reactionRepository.AddAsync(reaction);
            return ReactionDto.FromReaction(createdReaction);
        }
    }
}
