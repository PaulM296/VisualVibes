using AutoMapper;
using VisualVibes.App.DTOs.UserDtos;
using VisualVibes.Domain.Models;

namespace VisualVibes.App.MappingProfiles
{
    public class UserMappings : Profile
    {
        public UserMappings()
        {
            CreateMap<AppUser, ResponseUserDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.UserProfile.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.UserProfile.LastName))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.UserProfile.DateOfBirth))
                .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.UserProfile.Bio))
                .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.UserProfile.ProfilePicture));
        }
    }
}
