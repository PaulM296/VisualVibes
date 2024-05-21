using MediatR;
using VisualVibes.App.DTOs.MessageDtos;
using VisualVibes.App.DTOs.PaginationDtos;

namespace VisualVibes.App.Messages.Queries
{
    public record GetAllConversationMessagesQuery(Guid ConversationId, PaginationRequestDto paginationRequestDto) : IRequest<PaginationResponseDto<ResponseMessageDto>>;
}
