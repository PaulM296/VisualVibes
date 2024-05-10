using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.DTOs.ReactionDtos;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Reactions.Commands;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Reactions.CommandsHandler
{
    public class CreateReactionCommandHandler : IRequestHandler<CreateReactionCommand, ResponseReactionDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateReactionCommandHandler> _logger;
        private readonly IMapper _mapper;

        public CreateReactionCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateReactionCommandHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<ResponseReactionDto> Handle(CreateReactionCommand request, CancellationToken cancellationToken)
        {
            var reaction = new Reaction()
            {
                UserId = request.createReactionDto.UserId,
                PostId = request.createReactionDto.PostId,
                ReactionType = request.createReactionDto.ReactionType,
                Timestamp = DateTime.UtcNow,
            };

            var createdReaction = await _unitOfWork.ReactionRepository.AddAsync(reaction);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Reaction successfully added!");

            return _mapper.Map<ResponseReactionDto>(createdReaction);
        }
    }
}
