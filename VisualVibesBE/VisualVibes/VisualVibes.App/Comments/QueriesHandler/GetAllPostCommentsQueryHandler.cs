using MediatR;
using VisualVibes.App.Comments.Queries;
using VisualVibes.App.DTOs.CommentDtos;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;

namespace VisualVibes.App.Comments.QueriesHandler
{
    public class GetAllPostCommentsQueryHandler : IRequestHandler<GetAllPostCommentsQuery, ICollection<ResponseCommentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllPostCommentsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ICollection<ResponseCommentDto>> Handle(GetAllPostCommentsQuery request, CancellationToken cancellationToken)
        {
            var comments = await _unitOfWork.CommentRepository.GetAllAsync(request.PostId);

            if (comments.Count == 0)
            {
                throw new CommentsNotFoundException($"Could not get the comments from PostId {request.PostId}, because it doesn't have any yet!");
            }

            var commentDtos = comments.Select(ResponseCommentDto.FromComment).ToList();

            return commentDtos;
        }
    }
}
