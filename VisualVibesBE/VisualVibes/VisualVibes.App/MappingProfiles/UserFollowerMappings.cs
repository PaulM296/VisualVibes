using AutoMapper;
using VisualVibes.App.DTOs.UserFollowerDtos;
using VisualVibes.Domain.Models;

namespace VisualVibes.App.MappingProfiles
{
    public class UserFollowerMappings : Profile
    {
        public UserFollowerMappings()
        {
            CreateMap<UserFollower, UserFollowerDto>();
        }
    }
}
