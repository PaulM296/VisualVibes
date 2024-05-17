using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.DTOs.UserDtos;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Users.Queries;

namespace VisualVibes.App.Users.QueriesHandler
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ResponseUserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetUserByIdQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetUserByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetUserByIdQueryHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ResponseUserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(request.UserId);

            if (user == null)
            {
                throw new UserNotFoundException($"Could not get the user with Id {request.UserId}, because it doesn't exist!");
            }

            _logger.LogInformation($"User successfully retrieved!");

            return _mapper.Map<ResponseUserDto>(user);
        }
    }
}
