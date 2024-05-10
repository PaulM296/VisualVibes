using AutoMapper;
using VisualVibes.App.DTOs.CommentDtos;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.MappingProfiles
{
    public class CommentMappings : Profile
    {
        public CommentMappings()
        {
            CreateMap<Comment, ResponseCommentDto>();
        }
    }
}
