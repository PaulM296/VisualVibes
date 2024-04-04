using MediatR;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Reactions.Commands;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Reactions.CommandsHandler
{
    public class RemoveReactionCommandHandler : IRequestHandler<RemoveReactionCommand, Unit>
    {
        private readonly IReactionRepository _reactionRepository;
        public RemoveReactionCommandHandler(IReactionRepository reactionRepository) 
        {
            _reactionRepository = reactionRepository;
        }
        public async Task<Unit> Handle(RemoveReactionCommand request, CancellationToken cancellationToken)
        {
            var reaction = new Reaction()
            {
                Id = request.Id
            };
            await _reactionRepository.RemoveAsync(reaction);
            return Unit.Value;
        }
    }
}
