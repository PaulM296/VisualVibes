using AutoMapper;
using VisualVibes.App.DTOs.CommentDtos;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.MappingProfiles
{
    public class CommentMappings : Profile
    {
        public CommentMappings()
        {
            CreateMap<Comment, ResponseCommentDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.ImageId, opt => opt.MapFrom(src => src.User.UserProfile.ImageId));
        }
    }
}
