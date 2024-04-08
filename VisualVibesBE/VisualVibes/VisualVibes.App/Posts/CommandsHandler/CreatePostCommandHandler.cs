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
        public readonly IPostRepository _postRepository;
        public CreatePostCommandHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        public async Task<PostDto> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var post = new Post()
            {
                Id = Guid.NewGuid(),
                UserId = request.PostDto.UserId,
                Caption = request.PostDto.Caption,
                Pictures = request.PostDto.Pictures,
                CreatedAt = request.PostDto.CreatedAt
            };
            var createdPost = await _postRepository.AddAsync(post);
            return PostDto.FromPost(createdPost);
        }
    }
}
