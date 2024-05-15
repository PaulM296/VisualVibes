using AutoMapper;
using VisualVibes.App.DTOs.UserProfileDtos;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.MappingProfiles
{
    public class UserProfileMappings : Profile
    {
        public UserProfileMappings()
        {
            CreateMap<UserProfile, ResponseUserProfileDto>();
        }
    }
}
