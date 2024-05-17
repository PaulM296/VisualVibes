using AutoMapper;
using VisualVibes.App.DTOs.UserDtos;
using VisualVibes.Domain.Models;

namespace VisualVibes.App.MappingProfiles
{
    public class UserMappings : Profile
    {
        public UserMappings()
        {
            CreateMap<AppUser, ResponseUserDto>();
        }
    }
}
