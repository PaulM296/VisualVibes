using MediatR;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Reactions.Queries;

namespace VisualVibes.App.Reactions.QueriesHandler
{
    public class GetAllPostReactionsCommandHandler : IRequestHandler<GetAllPostReactionsCommand, ICollection<ReactionDto>>
    {
        public readonly IReactionRepository _reactionRepository;

        public GetAllPostReactionsCommandHandler(IReactionRepository reactionRepository)
        {
            _reactionRepository = reactionRepository;
        }

        public async Task<ICollection<ReactionDto>> Handle(GetAllPostReactionsCommand request, CancellationToken cancellationToken)
        {
            var reactions = await _reactionRepository.GetAllAsync(request.PostId);

            if (reactions.Count == 0)
            {
                throw new ApplicationException("Reactions not found");
            }

            var reactionDtos = new List<ReactionDto>();
            foreach (var reaction in reactions)
            {
                reactionDtos.Add(ReactionDto.FromReaction(reaction));
            }

            return reactionDtos;
        }
    }
}
