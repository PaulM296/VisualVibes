using MediatR;
using VisualVibes.App.Feeds.Commands;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Feeds.CommandsHandlers
{
    public class CreateFeedCommandHandler : IRequestHandler<CreateFeedCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateFeedCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(CreateFeedCommand request, CancellationToken cancellationToken)
        {
            var feed = new Feed()
            {
                UserID = request.FeedDto.UserID
            };

            await _unitOfWork.FeedRepository.AddAsync(feed);
            await _unitOfWork.SaveAsync();

            return Unit.Value;
        }
    }
}
