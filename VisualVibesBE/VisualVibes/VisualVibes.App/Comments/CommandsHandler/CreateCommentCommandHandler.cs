using MediatR;
using VisualVibes.App.Comments.Commands;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Users.Queries;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Comments.CommandsHandler
{
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, CommentDto>
    {
        private readonly ICommentRepository _commentRepository;
        
        public CreateCommentCommandHandler(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }
            
        public async Task<CommentDto> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = new Comment()
            {
                Id = Guid.NewGuid(),
                UserId = request.CommentDto.UserId,
                PostId = request.CommentDto.PostId,
                Text = request.CommentDto.Text,
                CreatedAt = request.CommentDto.CreatedAt
            };
            var createdComment = await _commentRepository.AddAsync(comment);
            return CommentDto.FromComment(createdComment);
        }
    }
}
