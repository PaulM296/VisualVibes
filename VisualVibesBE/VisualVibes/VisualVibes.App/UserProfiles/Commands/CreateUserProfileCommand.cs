using MediatR;
using VisualVibes.App.DTOs.UserProfileDtos;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.UserProfiles.Commands
{
    public record CreateUserProfileCommand(CreateUserProfileDto createUserProfileDto) : IRequest<ResponseUserProfileDto>;

}
