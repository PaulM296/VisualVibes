using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisualVibes.Api.Extensions;
using VisualVibes.App.Comments.Commands;
using VisualVibes.App.Comments.Queries;
using VisualVibes.App.DTOs.CommentDtos;
using VisualVibes.App.DTOs.PaginationDtos;
using VisualVibes.App.Reactions.Queries;

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
            var userId = HttpContext.GetUserIdClaimValue();

            var createComment = await _mediator.Send(new CreateCommentCommand(userId, createCommentDto));

            return Ok(createComment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveComment(Guid id)
        {
            var userId = HttpContext.GetUserIdClaimValue();

            var deleteComment = await _mediator.Send(new RemoveCommentCommand(userId, id));

            return Ok(deleteComment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCommnet(Guid id, UpdateCommentDto updateCommentDto)
        {
            var userId = HttpContext.GetUserIdClaimValue();

            var updateComment = await _mediator.Send(new UpdateCommentCommand(userId, id, updateCommentDto));

            return Ok(updateComment);
        }

        [HttpGet("post/{id}")]
        public async Task<IActionResult> GetAllPostComments(Guid id, [FromQuery]PaginationRequestDto paginationRequestDto)
        {
            var postComments = await _mediator.Send(new GetAllPostCommentsQuery(id, paginationRequestDto));
            
            return Ok(postComments);
        }

        [HttpGet("post/{postId}/comments/total")]
        public async Task<IActionResult> GetAllPostCommentsNumber(Guid postId)
        {
            var totalPostComments = await _mediator.Send(new GetTotalCommentNumberForPostQuery(postId));

            return Ok(totalPostComments);
        }
    }
}
