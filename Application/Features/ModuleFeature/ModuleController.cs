using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SetelaServerV3._1.Application.Features.ModuleFeature.Commands.CreateModuleCommand;
using SetelaServerV3._1.Application.Features.ModuleFeature.Commands.DeleteModuleCommand;
using SetelaServerV3._1.Application.Features.ModuleFeature.Commands.UpdateModuleCommand;
using SetelaServerV3._1.Application.Features.ModuleFeature.DTO;
using SetelaServerV3._1.Application.Features.ModuleFeature.Queries.GetModuleByIdQuery;
using SetelaServerV3._1.Shared.Common.DTO;
using SetelaServerV3._1.Shared.Utilities;
using System.Security.Claims;

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

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<ModuleSimpleDTO>> UpdateModule([FromBody] UpdateModuleRequestDTO request, int id)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var response = await _mediator.Send(new UpdateModuleCommand
            {
                UserId = int.Parse(userId),
                ModuleId = id,
                Title = request.Title,
                TextContent = request.TextContent,
                Visible = request.Visible
            });

            return response.ToActionResult();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<object>> DeleteModule(int id)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var response = await _mediator.Send(new DeleteModuleCommand
            {
                UserId = int.Parse(userId),
                ModuleId = id
            });

            return response.ToActionResult();
        }
    }
}
