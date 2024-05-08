using MediatR;
using VisualVibes.App.DTOs.UserDtos;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Users.Queries;

namespace VisualVibes.App.Users.QueriesHandler
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ResponseUserDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseUserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);

            if (user == null)
            {
                throw new UserNotFoundException($"Could not get the user with Id {request.UserId}, because it doesn't exist!");
            }

            return ResponseUserDto.FromUser(user);
        }
    }
}
