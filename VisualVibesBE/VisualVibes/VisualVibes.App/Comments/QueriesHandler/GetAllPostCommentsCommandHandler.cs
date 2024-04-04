using MediatR;
using VisualVibes.App.Comments.Queries;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;

namespace VisualVibes.App.Comments.QueriesHandler
{
    public class GetAllCommentsCommandHandler : IRequestHandler<GetAllPostCommentsCommand, ICollection<CommentDto>>
    {
        private readonly ICommentRepository _commentRepository;

        public GetAllCommentsCommandHandler(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<ICollection<CommentDto>> Handle(GetAllPostCommentsCommand request, CancellationToken cancellationToken)
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
