using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using VisualVibes.App.DTOs.MessageDtos;
using VisualVibes.App.Messages.Commands;
using VisualVibes.App.Messages.Queries;

namespace VisualVibes.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            var createMessage = await _mediator.Send(new CreateMessageCommand(createMessageDto));

            return Ok(createMessage);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveMessage(Guid id)
        {
            var removeMessage = await _mediator.Send(new RemoveMessageCommand(id));

            return Ok(removeMessage);
        }

        [HttpGet("conversation/{id}")]
        public async Task<IActionResult> GetAllConversationMessages(Guid id)
        {
            var messages = await _mediator.Send(new GetAllConversationMessagesQuery(id));

            return Ok(messages);
        }


    }
}
