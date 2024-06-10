using AutoMapper;
using VisualVibes.App.DTOs.ReactionDtos;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.MappingProfiles
{
    public class ReactionMappings : Profile
    {
        public ReactionMappings()
        {
            CreateMap<Reaction, ResponseReactionDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.ImageId, opt => opt.MapFrom(src => src.User.UserProfile.ImageId));
        }
    }
}
