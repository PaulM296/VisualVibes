using MediatR;
using VisualVibes.App.DTOs;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.UserProfiles.Commands
{
    public record UpdateUserProfileCommand(UserProfileDto UserProfileDto) : IRequest<UserProfileDto>;
}
