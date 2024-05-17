using MediatR;
using VisualVibes.App.DTOs.UserProfileDtos;

namespace VisualVibes.App.UserProfiles.Commands
{
    public record CreateUserProfileCommand(CreateUserProfileDto createUserProfileDto) : IRequest<ResponseUserProfileDto>;

}
