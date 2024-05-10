using AutoMapper;
using VisualVibes.App.DTOs.ConversationDtos;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.MappingProfiles
{
    public class ConversationMappings : Profile
    {
        public ConversationMappings()
        {
            CreateMap<Conversation, ResponseConversationDto>();
        }
    }
}
