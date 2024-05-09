using MediatR;
using VisualVibes.App.DTOs.UserProfileDtos;

namespace VisualVibes.App.UserProfiles.Queries
{
    public record GetUserProfileByUserIdQuery(Guid userId) : IRequest<ResponseUserProfileDto>;
}
