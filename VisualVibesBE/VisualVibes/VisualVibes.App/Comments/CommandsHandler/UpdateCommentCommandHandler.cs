using MediatR;
using VisualVibes.App.Comments.Commands;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Comments.CommandsHandler
{
    public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, CommentDto>
    {
        private readonly ICommentRepository _commentRepository;

        public UpdateCommentCommandHandler(ICommentRepository commentRepository) 
        {
            _commentRepository = commentRepository;
        }
        public async Task<CommentDto> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = new Comment()
            {
                Id = request.CommentDto.Id,
                UserId = request.CommentDto.UserId,
                Text = request.CommentDto.Text,
                CreatedAt = request.CommentDto.CreatedAt
            };
            var updatedComment = await _commentRepository.UpdateAsync(comment);
            return CommentDto.FromComment(updatedComment);
        }
    }
}