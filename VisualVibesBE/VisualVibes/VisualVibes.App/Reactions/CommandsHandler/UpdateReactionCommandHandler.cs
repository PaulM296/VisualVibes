using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.DTOs.ReactionDtos;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Reactions.Commands;

namespace VisualVibes.App.Reactions.CommandsHandler
{
    public class UpdateReactionCommandHandler : IRequestHandler<UpdateReactionCommand, ResponseReactionDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateReactionCommandHandler> _logger;
        private readonly IMapper _mapper;

        public UpdateReactionCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateReactionCommandHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<ResponseReactionDto> Handle(UpdateReactionCommand request, CancellationToken cancellationToken)
        {
            var getReaction = await _unitOfWork.ReactionRepository.GetByIdAsync(request.id);

            if(getReaction == null)
            {
                throw new ReactionNotFoundException($"The reaction with ID {request.id} doesn't exist and it could not be updated!");
            }

            getReaction.ReactionType = request.updateReactionDto.ReactionType;

            var updatedReaction = await _unitOfWork.ReactionRepository.UpdateAsync(getReaction);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Reaction successfully updated!");

            return _mapper.Map<ResponseReactionDto>(updatedReaction);
        }
    }
}
