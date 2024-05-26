using AutoMapper;
using VisualVibes.App.DTOs.ImageDtos;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.MappingProfiles
{
    public class ImageMappings : Profile
    {
        public ImageMappings()
        {
            CreateMap<Image, ImageResponseDto>();
        }
    }
}
