using AutoMapper;
using VisualVibes.App.DTOs.UserDtos;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.MappingProfiles
{
    public class UserMappings : Profile
    {
        public UserMappings()
        {
            CreateMap<User, ResponseUserDto>();
        }
    }
}
