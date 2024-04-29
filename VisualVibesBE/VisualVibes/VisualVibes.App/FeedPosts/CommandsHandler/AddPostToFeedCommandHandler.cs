using MediatR;
using VisualVibes.App.FeedPosts.Commands;
using VisualVibes.App.Interfaces;

namespace VisualVibes.App.FeedPosts.CommandsHandler
{
    public class AddPostToFeedCommandHandler : IRequestHandler<AddPostToFeedCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddPostToFeedCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(AddPostToFeedCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.FeedPostRepository.AddPostToFeedAsync(request.PostId);
            await _unitOfWork.SaveAsync();
            return Unit.Value;
        }
    }
}
