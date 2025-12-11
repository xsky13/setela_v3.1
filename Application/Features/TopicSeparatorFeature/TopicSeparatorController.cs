using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SetelaServerV3._1.Application.Features.TopicSeparatorFeature.Commands.CreateTopicSeparator;
using SetelaServerV3._1.Application.Features.TopicSeparatorFeature.Commands.DeleteTopicSeparator;
using SetelaServerV3._1.Application.Features.TopicSeparatorFeature.Commands.UpdateTopicSeparator;
using SetelaServerV3._1.Application.Features.TopicSeparatorFeature.DTO;
using SetelaServerV3._1.Shared.Utilities;
using System.Security.Claims;

namespace SetelaServerV3._1.Application.Features.TopicSeparatorFeature
{
    [ApiController]
    [Route("api/[controller]")]
    public class TopicSeparatorController(IMediator _mediator) : ControllerBase
    {
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<TopicSeparatorDTO>> CreateTopicSeparator([FromBody] CreateTopicSeparatorRequestDTO request)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var response = await _mediator.Send(new CreateTopicSeparatorCommand 
            {
                CurrentUserId = int.Parse(userId),
                Title = request.Title,
                CourseId = request.CourseId
            });

            return response.ToActionResult();
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<TopicSeparatorDTO>> UpdateTopicSeparator([FromBody] UpdateTopicSeparatorRequestDTO request, int id)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var response = await _mediator.Send(new UpdateTopicSeparatorCommand 
            { 
                Id = id, 
                NewTitle = request.NewTitle, 
                CurrentUserId = int.Parse(userId),
            });
            return response.ToActionResult();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<object>> DeleteTopicSeparator(int id)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var response = await _mediator.Send(new DeleteTopicSeparatorCommand 
            { 
                Id = id, 
                CurrentUserId = int.Parse(userId),
            });
            return response.ToActionResult();
        }
    }
}
