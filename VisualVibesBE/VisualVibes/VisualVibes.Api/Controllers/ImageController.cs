using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisualVibes.App.DTOs.ImageDtos;
using VisualVibes.App.Images.Commands;

namespace VisualVibes.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ImageController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ImageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage([FromForm] UploadImageDto uploadImageDto)
        {
            var command = new UploadImageCommand(uploadImageDto);
            var response = await _mediator.Send(command);

            return Ok(response);
        }

        [HttpDelete("id")]
        public async Task<IActionResult> RemoveImage(Guid id)
        {
            var command = new RemoveImageCommand(id);
            var response = await _mediator.Send(command);

            return Ok(response);
        } 
    }
}
