using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SetelaServerV3._1.Application.Features.AssignmentFeature.Commands.CreateAssignmentCommand;
using SetelaServerV3._1.Application.Features.AssignmentFeature.DTO;
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
        [HttpPost]
        public async Task<ActionResult<Assignment>> CreateAssignment([FromBody] CreateAssignmentRequestDTO request)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var response = await _mediator.Send(new CreateAssignmentCommand { UserId = int.Parse(userId), Assignment = request });
            return response.ToActionResult();
        }
    }
}
