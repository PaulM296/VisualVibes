using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.DTOs.UserProfileDtos;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;
using VisualVibes.App.UserProfiles.Queries;

namespace VisualVibes.App.UserProfiles.QueriesHandler
{
    public class GetUserProfileByUserIdQueryHandler : IRequestHandler<GetUserProfileByUserIdQuery, ResponseUserProfileDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetUserProfileByUserIdQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetUserProfileByUserIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetUserProfileByUserIdQueryHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ResponseUserProfileDto> Handle(GetUserProfileByUserIdQuery request, CancellationToken cancellationToken)
        {
            var userProfile = await _unitOfWork.UserProfileRepository.GetUserProfileByUserId(request.userId);

            if (userProfile == null)
            {
                throw new UserProfileNotFoundException($"The user profile with userId: {request.userId} has not been found!");
            }

            _logger.LogInformation("UserProfile successfully retrieved!");

            return _mapper.Map<ResponseUserProfileDto>(userProfile);
        }
    }
}
