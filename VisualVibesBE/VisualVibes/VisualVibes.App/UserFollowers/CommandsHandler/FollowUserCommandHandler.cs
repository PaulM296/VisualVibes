using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.Interfaces;
using VisualVibes.App.UserFollowers.Commands;

namespace VisualVibes.App.UserFollowers.CommandsHandler
{
    public class FollowUserCommandHandler : IRequestHandler<FollowUserCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<FollowUserCommandHandler> _logger;

        public FollowUserCommandHandler(IUnitOfWork unitOfWork, ILogger<FollowUserCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Unit> Handle(FollowUserCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.UserFollowerRepository.AddFollowerAsync(request.FollowerId, request.FollowingId);

            await _unitOfWork.SaveAsync();

            _logger.LogInformation("User successfully followed!");

            return Unit.Value;
        }
    }
}
