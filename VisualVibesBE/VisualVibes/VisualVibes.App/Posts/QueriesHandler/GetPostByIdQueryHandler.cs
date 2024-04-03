using MediatR;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Posts.Queries;

namespace VisualVibes.App.Posts.QueriesHandler
{
    public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, PostDto>
    {
        public readonly IPostRepository _postRepository;

        public GetPostByIdQueryHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<PostDto> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            var post = await _postRepository.GetByIdAsync(request.PostId);
            if (post == null)
            {
                throw new ApplicationException("Post not found");
            }
            return PostDto.FromPost(post);
        }
    }
}
