using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisualVibes.Api.Extensions;
using VisualVibes.App.DTOs.MessageDtos;
using VisualVibes.App.DTOs.PaginationDtos;
using VisualVibes.App.Messages.Commands;
using VisualVibes.App.Messages.Queries;

namespace VisualVibes.Api.Controllers
{
    [ApiController]
    [Route("api/messages")]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MessagesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(CreateMessageDto createMessageDto)
        {
            var userId = HttpContext.GetUserIdClaimValue();

            var createMessage = await _mediator.Send(new CreateMessageCommand(userId, createMessageDto));

            return Ok(createMessage);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveMessage(Guid id)
        {
            var userId = HttpContext.GetUserIdClaimValue();

            var removeMessage = await _mediator.Send(new RemoveMessageCommand(userId, id));

            return Ok(removeMessage);
        }

        [HttpGet("conversation/{id}")]
        public async Task<IActionResult> GetAllConversationMessages(Guid id, [FromQuery]PaginationRequestDto paginationRequestDto)
        {
            var messages = await _mediator.Send(new GetAllConversationMessagesQuery(id, paginationRequestDto));

            return Ok(messages);
        }


    }
}
