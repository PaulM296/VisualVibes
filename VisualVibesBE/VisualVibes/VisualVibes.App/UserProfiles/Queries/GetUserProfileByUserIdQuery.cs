using MediatR;
using VisualVibes.App.DTOs.UserProfileDtos;

namespace VisualVibes.App.UserProfiles.Queries
{
    public record GetUserProfileByUserIdQuery(string userId) : IRequest<ResponseUserProfileDto>;
}
