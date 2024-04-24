using MediatR;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Reactions.Queries;

namespace VisualVibes.App.Reactions.QueriesHandler
{
    public class GetAllPostReactionsQueryHandler : IRequestHandler<GetAllPostReactionsQuery, ICollection<ReactionDto>>
    {
        public readonly IUnitOfWork _unitOfWork;

        public GetAllPostReactionsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ICollection<ReactionDto>> Handle(GetAllPostReactionsQuery request, CancellationToken cancellationToken)
        {
            var reactions = await _unitOfWork.ReactionRepository.GetAllAsync(request.PostId);

            if (reactions.Count == 0)
            {
                throw new ApplicationException("Reactions not found");
            }

            var reactionDtos = reactions.Select(ReactionDto.FromReaction).ToList();

            return reactionDtos;
        }
    }
}
