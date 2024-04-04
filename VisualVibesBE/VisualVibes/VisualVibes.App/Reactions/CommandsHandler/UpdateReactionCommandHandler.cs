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
        private readonly IReactionRepository _reactionRepository;
        public UpdateReactionCommandHandler(IReactionRepository reactionRepository) 
        {
            _reactionRepository = reactionRepository;
        }
        public async Task<ReactionDto> Handle(UpdateReactionCommand request, CancellationToken cancellationToken)
        {
            var reaction = new Reaction()
            {
                Id = request.ReactionDto.Id,
                UserId = request.ReactionDto.UserId,
                PostId = request.ReactionDto.PostId,
                ReactionType = request.ReactionDto.ReactionType,
                Timestamp = request.ReactionDto.Timestamp
            };
            var createdReaction = await _reactionRepository.UpdateAsync(reaction);
            return ReactionDto.FromReaction(createdReaction);
        }
    }
}
