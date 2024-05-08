using MediatR;
using VisualVibes.App.DTOs.PostDtos;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Posts.Commands;
using VisualVibes.App.Users.Queries;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Posts.CommandsHandler
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, ResponsePostDto>
    {
        public readonly IUnitOfWork _unitOfWork;
        public CreatePostCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;  
        }
        public async Task<ResponsePostDto> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var post = new Post()
            {
                Id = Guid.NewGuid(),
                UserId = request.requestPostDto.UserId,
                Caption = request.requestPostDto.Caption,
                Pictures = request.requestPostDto.Pictures,
                CreatedAt = DateTime.UtcNow,
            };

            var createdPost = await _unitOfWork.PostRepository.AddAsync(post);
            await _unitOfWork.SaveAsync();

            return ResponsePostDto.FromPost(createdPost);
        }
    }
}
