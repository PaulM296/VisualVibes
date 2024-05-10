using AutoMapper;
using VisualVibes.App.DTOs.MessageDtos;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.MappingProfiles
{
    public class MessageMappings : Profile
    {
        public MessageMappings()
        {
            CreateMap<Message, ResponseMessageDto>();
        }
    }
}
