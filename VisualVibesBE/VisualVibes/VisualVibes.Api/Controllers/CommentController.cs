using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisualVibes.App.Comments.Commands;
using VisualVibes.App.Comments.Queries;
using VisualVibes.App.DTOs.CommentDtos;

namespace VisualVibes.Api.Controllers
{
    [ApiController]
    [Route("api/comments")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CommentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CommentController(IMediator mediator)
        {
            _mediator = mediator;            
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(CreateCommentDto createCommentDto)
        {
            var createComment = await _mediator.Send(new CreateCommentCommand(createCommentDto));

            return Ok(createComment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveComment(Guid id)
        {
            var deleteComment = await _mediator.Send(new RemoveCommentCommand(id));

            return Ok(deleteComment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCommnet(Guid id, UpdateCommentDto updateCommentDto)
        {
            var updateComment = await _mediator.Send(new UpdateCommentCommand(id, updateCommentDto));

            return Ok(updateComment);
        }

        [HttpGet("post/{id}")]
        public async Task<IActionResult> GetAllPostComments(Guid id)
        {
            var postComments = await _mediator.Send(new GetAllPostCommentsQuery(id));
            
            return Ok(postComments);
        }
    }
}
