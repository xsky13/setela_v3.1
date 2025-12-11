using MediatR;
using Microsoft.AspNetCore.Mvc;
using SetelaServerV3._1.Application.Features.TopicSeparatorFeature.Commands.CreateTopicSeparator;
using SetelaServerV3._1.Application.Features.TopicSeparatorFeature.Commands.DeleteTopicSeparator;
using SetelaServerV3._1.Application.Features.TopicSeparatorFeature.Commands.UpdateTopicSeparator;
using SetelaServerV3._1.Application.Features.TopicSeparatorFeature.DTO;
using SetelaServerV3._1.Shared.Utilities;

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

        [HttpPut("{id}")]
        public async Task<ActionResult<TopicSeparatorDTO>> UpdateTopicSeparator([FromBody] UpdateTopicSeparatorRequestDTO request, int id)
        {
            var response = await _mediator.Send(new UpdateTopicSeparatorCommand { Id = id, NewTitle = request.NewTitle });
            return response.ToActionResult();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<object>> DeleteTopicSeparator(int id)
        {
            var response = await _mediator.Send(new DeleteTopicSeparatorCommand { Id = id });
            return response.ToActionResult();
        }
    }
}
