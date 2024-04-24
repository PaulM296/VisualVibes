using MediatR;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Posts.Commands;
using VisualVibes.App.Users.Queries;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Posts.CommandsHandler
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, PostDto>
    {
        public readonly IUnitOfWork _unitOfWork;
        public CreatePostCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;  
        }
        public async Task<PostDto> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var post = new Post()
            {
                Id = request.PostDto.Id,
                UserId = request.PostDto.UserId,
                Caption = request.PostDto.Caption,
                Pictures = request.PostDto.Pictures,
                CreatedAt = request.PostDto.CreatedAt
            };

            var createdPost = await _unitOfWork.PostRepository.AddAsync(post);
            await _unitOfWork.SaveAsync();

            return PostDto.FromPost(createdPost);
        }
    }
}
