using MediatR;
using VisualVibes.App.Comments.Commands;
using VisualVibes.App.DTOs.CommentDtos;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Users.Queries;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Comments.CommandsHandler
{
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, ResponseCommentDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public CreateCommentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
            
        public async Task<ResponseCommentDto> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = new Comment()
            {
                UserId = request.createCommentDto.UserId,
                PostId = request.createCommentDto.PostId,
                Text = request.createCommentDto.Text,
                CreatedAt = DateTime.UtcNow
            };

            var createdComment = await _unitOfWork.CommentRepository.AddAsync(comment);
            await _unitOfWork.SaveAsync();

            return ResponseCommentDto.FromComment(createdComment);
        }
    }
}
