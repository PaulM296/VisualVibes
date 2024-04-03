using MediatR;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Users.Queries;

namespace VisualVibes.App.Users.QueriesHandler
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
    {
        public readonly IUserRepository _userRepository;

        public GetUserByIdQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.userId);
            
            if (user == null)
            {
                throw new ApplicationException("User not found!");
            }

            return UserDto.FromUser(user);
        }
    }
}
