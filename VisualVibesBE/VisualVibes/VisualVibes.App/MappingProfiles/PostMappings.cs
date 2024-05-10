using AutoMapper;
using VisualVibes.App.DTOs.PostDtos;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.MappingProfiles
{
    public class PostMappings : Profile
    {
        public PostMappings()
        {
            CreateMap<Post, ResponsePostDto>();
        }
    }
}
