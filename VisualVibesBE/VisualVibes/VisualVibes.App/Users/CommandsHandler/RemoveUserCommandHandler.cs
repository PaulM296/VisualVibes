using MediatR;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Users.Commands;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Users.CommandsHandler
{
    public class RemoveUserCommandHandler : IRequestHandler<RemoveUserCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
        {
            var userToRemove = await _unitOfWork.UserRepository.GetByIdAsync(request.Id);

            if (userToRemove == null)
            {
                throw new UserNotFoundException($"The user with ID {request.Id} doesn't exist and it could not be removed!");
            }

            await _unitOfWork.UserRepository.RemoveAsync(userToRemove);
            await _unitOfWork.SaveAsync();

            return Unit.Value;
        }
    }
}
