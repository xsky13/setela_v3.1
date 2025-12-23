using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SetelaServerV3._1.Application.Features.AssignmentFeature.Commands.CreateAssignmentCommand;
using SetelaServerV3._1.Application.Features.AssignmentSubmissionFeature.Commands.CreateAssignmentSubmissionCommand;
using SetelaServerV3._1.Application.Features.AssignmentSubmissionFeature.Commands.UpdateAssignmentSubmissionCommand;
using SetelaServerV3._1.Application.Features.AssignmentSubmissionFeature.DTO;
using SetelaServerV3._1.Shared.Utilities;
using System.Security.Claims;

namespace SetelaServerV3._1.Application.Features.AssignmentSubmissionFeature
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssignmentSubmissionController(IMediator _mediator) : ControllerBase
    {
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<AssignmentSubmissionDTO>> Create([FromBody] CreateAssignmentSubmissionRequestDTO request)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var response = await _mediator.Send(new CreateAssignmentSubmissionCommand
            {
                UserId = int.Parse(userId),
                AssignmentSubmission = request
            });

            return response.ToActionResult();
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<AssignmentSubmissionDTO>> Update([FromBody] UpdateAssignmentSubmissionRequestDTO request, int id)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var response = await _mediator.Send(new UpdateAssignmentSubmissionCommand
            {
                UserId = int.Parse(userId),
                AssignmentSubmissionId = id,
                AssignmentSubmission = request
            });

            return response.ToActionResult();
        }
    }
}
