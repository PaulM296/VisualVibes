using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.DTOs.UserDtos;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Users.Queries;
using VisualVibes.Domain.Enum;

namespace VisualVibes.App.Users.QueriesHandler
{
    public class SearchUsersQueryHandler : IRequestHandler<SearchUsersQuery, IEnumerable<ResponseUserDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SearchUsersQueryHandler> _logger;
        private readonly IMapper _mapper;

        public SearchUsersQueryHandler(IUnitOfWork unitOfWork, ILogger<SearchUsersQueryHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ResponseUserDto>> Handle(SearchUsersQuery request, CancellationToken cancellationToken)
        {
            var query = request.query.ToLower();
            var users = await _unitOfWork.UserRepository
                .FindAsync(u => (u.UserName.ToLower().Contains(query) ||
                                u.UserProfile.FirstName.ToLower().Contains(query) ||
                                u.UserProfile.LastName.ToLower().Contains(query)) &&
                                u.Role == Role.User);

            if (!users.Any())
            {
                _logger.LogInformation("No users found matching the query.");
                return Enumerable.Empty<ResponseUserDto>();
            }

            var userDtos = _mapper.Map<IEnumerable<ResponseUserDto>>(users);

            _logger.LogInformation("Users successfully retrieved.");
            return userDtos;
        }
    }
}
