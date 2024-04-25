using MediatR;
using VisualVibes.App.Conversations.Commands;
using VisualVibes.App.DTOs;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Conversations.CommandsHandler
{
    public class RemoveConversationCommandHandler : IRequestHandler<RemoveConversationCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        public RemoveConversationCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(RemoveConversationCommand request, CancellationToken cancellationToken)
        {
            var conversationToRemove = await _unitOfWork.ConversationRepository.GetByIdAsync(request.Id);

            if(conversationToRemove == null)
            {
                throw new ConversationNotFoundException($"The conversation with ID {request.Id} doesn't exist and it could not be removed!");
            }

            await _unitOfWork.ConversationRepository.RemoveAsync(conversationToRemove);
            await _unitOfWork.SaveAsync();

            return Unit.Value;
        }
    }
}
