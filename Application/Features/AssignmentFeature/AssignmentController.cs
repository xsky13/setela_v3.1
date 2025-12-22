using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SetelaServerV3._1.Application.Features.AssignmentFeature.Commands.CreateAssignmentCommand;
using SetelaServerV3._1.Application.Features.AssignmentFeature.Commands.DeleteAssignmentCommand;
using SetelaServerV3._1.Application.Features.AssignmentFeature.Commands.UpdateAssignmentCommand;
using SetelaServerV3._1.Application.Features.AssignmentFeature.DTO;
using SetelaServerV3._1.Application.Features.AssignmentFeature.Queries.GetAssignmentByIdQuery;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Shared.Utilities;
using System.Security.Claims;

namespace SetelaServerV3._1.Application.Features.AssignmentFeature
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssignmentController(IMediator _mediator) : ControllerBase
    {
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<AssignmentDTO>> GetById(int id)
        {
            var response = await _mediator.Send(new GetAssignmentByIdQuery { AssignmentId = id });
            return response.ToActionResult();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Assignment>> CreateAssignment([FromBody] CreateAssignmentRequestDTO request)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var response = await _mediator.Send(new CreateAssignmentCommand { UserId = int.Parse(userId), Assignment = request });
            return response.ToActionResult();
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<AssignmentDTO>> UpdateAssignment([FromBody] UpdateAssignmentRequestDTO request, int id)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var response = await _mediator.Send(new UpdateAssignmentCommand
            {
                UserId = int.Parse(userId),
                AssignmentId = id,
                Assignment = request
            });

            return response.ToActionResult();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<object>> DeleteAssignment(int id)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var response = await _mediator.Send(new DeleteAssignmentCommand
            {
                UserId = int.Parse(userId),
                AssignmentId = id,
            });

            return response.ToActionResult();
        }
    }
}
