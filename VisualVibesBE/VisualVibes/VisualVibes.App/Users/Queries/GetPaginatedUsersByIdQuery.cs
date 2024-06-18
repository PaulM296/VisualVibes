using MediatR;
using VisualVibes.App.DTOs.PaginationDtos;
using VisualVibes.App.DTOs.UserDtos;
using VisualVibes.Domain.Models;

namespace VisualVibes.App.Users.Queries
{
    public record GetPaginatedUsersByIdQuery(PaginationRequestDto paginationRequest) : IRequest<PaginationResponseDto<ResponseUserDto>>;
}
