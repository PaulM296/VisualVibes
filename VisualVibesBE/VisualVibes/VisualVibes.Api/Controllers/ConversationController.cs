using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisualVibes.Api.Extensions;
using VisualVibes.App.Conversations.Commands;
using VisualVibes.App.Conversations.Queries;
using VisualVibes.App.DTOs.ConversationDtos;
using VisualVibes.App.DTOs.PaginationDtos;

namespace VisualVibes.Api.Controllers
{
    [ApiController]
    [Route("api/conversations")]
    [Authorize]
    public class ConversationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ConversationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateConversation(CreateConversationDto requestConversationDto)
        {
            var userId = HttpContext.GetUserIdClaimValue();

            var response = await _mediator.Send(new CreateConversationCommand(userId, requestConversationDto));

            return Ok(response);
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetAllUserConversations([FromQuery]PaginationRequestDto paginationRequestDto)
        {
            var userId = HttpContext.GetUserIdClaimValue();

            var conversations = await _mediator.Send(new GetAllUserConversationsQuery(userId, paginationRequestDto));

            return Ok(conversations);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveConversation(Guid id)
        {
            var userId = HttpContext.GetUserIdClaimValue();

            var response = await _mediator.Send(new RemoveConversationCommand(userId, id));

            return Ok(response);
        }
    }
}
