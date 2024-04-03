using MediatR;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Posts.Commands;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Posts.CommandsHandler
{
    public class RemovePostCommandHandler : IRequestHandler<RemovePostCommand, Unit>
    {
        private readonly IPostRepository _postRepository;

        public RemovePostCommandHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<Unit> Handle(RemovePostCommand request, CancellationToken cancellationToken)
        {
            var post = new Post
            {
                Id = request.Id
            };
            await _postRepository.RemoveAsync(post);
            return Unit.Value;
        }
    }
}
