using AutoMapper;
using VisualVibes.App.DTOs.UserProfileDtos;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.MappingProfiles
{
    public class UserProfileMappings : Profile
    {
        public UserProfileMappings()
        {
            CreateMap<User, ResponseUserProfileDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserProfile.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.UserProfile.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.UserProfile.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.UserProfile.Email))
                .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.UserProfile.ProfilePicture))
                .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.UserProfile.Bio))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.UserProfile.DateOfBirth));
        }
    }
}
