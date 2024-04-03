using MediatR;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Users.Queries;
using VisualVibes.App.Users.Responses;

namespace VisualVibes.App.Users.QueriesHandler
{
    public class GetUserByIdHandler : IRequestHandler<GetUserById, UserDto>
    {
        public readonly IUserRepository _userRepository;

        public GetUserByIdHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<UserDto> Handle(GetUserById request, CancellationToken cancellationToken)
        {
            var user = _userRepository.GetById(request.userId);
            if(user == null)
            {
                throw new ApplicationException("User not found!");
            }
            return Task.FromResult(UserDto.FromUser(user));
        }
    }
}
