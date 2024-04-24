using MediatR;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Messages.Commands;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Messages.CommandsHandler
{
    public class RemoveMessageCommandHandler : IRequestHandler<RemoveMessageCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveMessageCommandHandler(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(RemoveMessageCommand request, CancellationToken cancellationToken)
        {
            var messageToRemove = await _unitOfWork.MessageRepository.GetByIdAsync(request.Id);

            if (messageToRemove == null)
            {
                throw new Exception($"User with ID {request.Id} not found.");
            };

            await _unitOfWork.MessageRepository.RemoveAsync(messageToRemove);
            await _unitOfWork.SaveAsync();

            return Unit.Value;

        }
    }
}
