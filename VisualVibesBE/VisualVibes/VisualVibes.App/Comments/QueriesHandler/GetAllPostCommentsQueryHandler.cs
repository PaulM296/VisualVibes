using MediatR;
using VisualVibes.App.Comments.Queries;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;

namespace VisualVibes.App.Comments.QueriesHandler
{
    public class GetAllPostCommentsQueryHandler : IRequestHandler<GetAllPostCommentsQuery, ICollection<CommentDto>>
    {
        private readonly ICommentRepository _commentRepository;

        public GetAllPostCommentsQueryHandler(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<ICollection<CommentDto>> Handle(GetAllPostCommentsQuery request, CancellationToken cancellationToken)
        {
            var comments = await _commentRepository.GetAllAsync(request.PostId);

            if (comments.Count == 0)
            {
                throw new ApplicationException("Comments not found");
            }

            var commentDtos = new List<CommentDto>();
            foreach (var comment in comments)
            {
                commentDtos.Add(CommentDto.FromComment(comment));
            }

            return commentDtos;
        }
    }
}
