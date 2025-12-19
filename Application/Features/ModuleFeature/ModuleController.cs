using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SetelaServerV3._1.Application.Features.ModuleFeature.Commands.CreateModuleCommand;
using SetelaServerV3._1.Application.Features.ModuleFeature.DTO;
using SetelaServerV3._1.Application.Features.ModuleFeature.Queries.GetModuleByIdQuery;
using SetelaServerV3._1.Shared.Common.DTO;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.ModuleFeature
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModuleController(IMediator _mediator) : ControllerBase
    {
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ModuleSimpleDTO>> CreateModule([FromBody] CreateModuleCommand command)
        {
            var response = await _mediator.Send(command);
            return response.ToActionResult();
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ModuleDTO>> GetModuleById(int id)
        {
            var response = await _mediator.Send(new GetModuleByIdQuery { ModuleId = id });
            return response.ToActionResult();
        }
    }
}
