using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisualVibes.Api.Extensions;
using VisualVibes.App.Conversations.Commands;
using VisualVibes.App.Conversations.Queries;
using VisualVibes.App.DTOs.ConversationDtos;

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
            var response = await _mediator.Send(new CreateConversationCommand(requestConversationDto));

            return Ok(response);
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetAllUserConversations()
        {
            var userId = HttpContext.GetUserIdClaimValue();

            var conversations = await _mediator.Send(new GetAllUserConversationsQuery(userId));

            return Ok(conversations);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveConversation(Guid id)
        {
            var response = await _mediator.Send(new RemoveConversationCommand(id));

            return Ok(response);
        }
    }
}
