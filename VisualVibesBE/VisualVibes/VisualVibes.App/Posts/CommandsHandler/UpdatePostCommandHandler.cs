using MediatR;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Posts.Commands;
using VisualVibes.App.Users.Queries;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Posts.CommandsHandler
{
    public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, PostDto>
    {
        private readonly IPostRepository _postRepository;
        public UpdatePostCommandHandler(IPostRepository postRepository) 
        {
            _postRepository = postRepository;
        }

        public async Task<PostDto> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            var post = new Post()
            {
                Id = request.PostDto.Id,
                UserId = request.PostDto.UserId,
                Caption = request.PostDto.Caption,
                Pictures = request.PostDto.Pictures,
                CreatedAt = request.PostDto.CreatedAt
            };
            var updatedPost = await _postRepository.UpdateAsync(post);
            return PostDto.FromPost(updatedPost);
        }
    }
}
