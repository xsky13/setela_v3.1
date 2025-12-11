using MediatR;
using Microsoft.AspNetCore.Mvc;
using SetelaServerV3._1.Application.Features.TopicSeparatorFeature.Commands.CreateTopicSeparator;
using SetelaServerV3._1.Application.Features.TopicSeparatorFeature.DTO;

namespace SetelaServerV3._1.Application.Features.TopicSeparatorFeature
{
    [ApiController]
    [Route("api/[controller]")]
    public class TopicSeparatorController(IMediator _mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<TopicSeparatorDTO>> CreateTopicSeparator([FromBody] CreateTopicSeparatorCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
