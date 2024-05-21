using MediatR;
using VisualVibes.App.DTOs.ConversationDtos;
using VisualVibes.App.DTOs.PaginationDtos;

namespace VisualVibes.App.Conversations.Queries
{
    public record GetAllUserConversationsQuery(string UserId, PaginationRequestDto paginationRequestDto) : IRequest<PaginationResponseDto<ResponseConversationDto>>;
}
