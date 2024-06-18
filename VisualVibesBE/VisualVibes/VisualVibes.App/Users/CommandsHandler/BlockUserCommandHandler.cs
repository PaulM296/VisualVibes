using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Users.Commands;

namespace VisualVibes.App.Users.CommandsHandler
{
    public class BlockUserCommandHandler : IRequestHandler<BlockUserCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BlockUserCommandHandler> _logger;

        public BlockUserCommandHandler(IUnitOfWork unitOfWork, ILogger<BlockUserCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Unit> Handle(BlockUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(request.userId);

            if(user == null)
            {
                throw new UserNotFoundException($"User with ID {request.userId} has not been found!");
            }

            user.isBlocked = true;
            await _unitOfWork.UserRepository.UpdateUserAsync(user);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation($"User with ID {request.userId} has been blocked!");

            return Unit.Value;
        }
    }
}
