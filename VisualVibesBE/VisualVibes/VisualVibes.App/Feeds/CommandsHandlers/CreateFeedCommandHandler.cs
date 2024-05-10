using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.Feeds.Commands;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Feeds.CommandsHandlers
{
    public class CreateFeedCommandHandler : IRequestHandler<CreateFeedCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateFeedCommandHandler> _logger;

        public CreateFeedCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateFeedCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<Unit> Handle(CreateFeedCommand request, CancellationToken cancellationToken)
        {
            var feed = new Feed()
            {
                UserID = request.FeedDto.UserID
            };

            await _unitOfWork.FeedRepository.AddAsync(feed);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Feed created successfully!");

            return Unit.Value;
        }
    }
}
