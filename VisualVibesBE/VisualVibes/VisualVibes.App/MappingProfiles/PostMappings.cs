using AutoMapper;
using VisualVibes.App.DTOs.CommentDtos;
using VisualVibes.App.DTOs.PostDtos;
using VisualVibes.App.DTOs.ReactionDtos;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.MappingProfiles
{
    public class PostMappings : Profile
    {
        public PostMappings()
        {
            CreateMap<Post, JoinedResponsePostDto>()
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))
                .ForMember(dest => dest.Reactions, opt => opt.MapFrom(src => src.Reactions));

            CreateMap<Comment, ResponseCommentDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.ImageId, opt => opt.MapFrom(src => src.User.UserProfile.ImageId));

            CreateMap<Reaction, ResponseReactionDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.ImageId, opt => opt.MapFrom(src => src.User.UserProfile.ImageId));

            CreateMap<Post, ResponsePostDto>();
        }
    }
}
