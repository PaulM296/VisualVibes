using MediatR;
using VisualVibes.App.DTOs;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.UserProfiles.Commands
{
    public record CreateUserProfileCommand(UserProfileDto UserProfileDto) : IRequest<UserProfileDto>;

}
