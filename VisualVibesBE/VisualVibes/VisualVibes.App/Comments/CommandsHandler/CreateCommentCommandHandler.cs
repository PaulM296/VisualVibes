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
        private readonly IUnitOfWork _unitOfWork;
        
        public CreateCommentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
            
        public async Task<CommentDto> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = new Comment()
            {
                Id = request.CommentDto.Id,
                UserId = request.CommentDto.UserId,
                PostId = request.CommentDto.PostId,
                Text = request.CommentDto.Text,
                CreatedAt = request.CommentDto.CreatedAt
            };

            var createdComment = await _unitOfWork.CommentRepository.AddAsync(comment);
            await _unitOfWork.SaveAsync();

            return CommentDto.FromComment(createdComment);
        }
    }
}
