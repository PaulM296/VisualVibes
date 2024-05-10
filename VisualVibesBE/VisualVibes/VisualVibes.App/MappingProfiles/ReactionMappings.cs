using AutoMapper;
using VisualVibes.App.DTOs.ReactionDtos;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.MappingProfiles
{
    public class ReactionMappings : Profile
    {
        public ReactionMappings()
        {
            CreateMap<Reaction, ResponseReactionDto>();
        }
    }
}
