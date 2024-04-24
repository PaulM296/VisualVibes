using MediatR;
using VisualVibes.App.Comments.Queries;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;

namespace VisualVibes.App.Comments.QueriesHandler
{
    public class GetAllPostCommentsQueryHandler : IRequestHandler<GetAllPostCommentsQuery, ICollection<CommentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllPostCommentsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ICollection<CommentDto>> Handle(GetAllPostCommentsQuery request, CancellationToken cancellationToken)
        {
            var comments = await _unitOfWork.CommentRepository.GetAllAsync(request.PostId);

            if (comments.Count == 0)
            {
                throw new ApplicationException("Comments not found");
            }

            var commentDtos = comments.Select(CommentDto.FromComment).ToList();

            return commentDtos;
        }
    }
}
