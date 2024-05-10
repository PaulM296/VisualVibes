using AutoMapper;
using VisualVibes.App.DTOs.FeedDtos;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.MappingProfiles
{
    public class FeedMappings : Profile
    {
        public FeedMappings()
        {
            CreateMap<Feed, CreateFeedDto>();
        }
    }
}
