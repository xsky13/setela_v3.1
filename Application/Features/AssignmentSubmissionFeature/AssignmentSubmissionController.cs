using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SetelaServerV3._1.Application.Features.AssignmentFeature.Commands.CreateAssignmentCommand;
using SetelaServerV3._1.Application.Features.AssignmentSubmissionFeature.Commands.CreateAssignmentSubmissionCommand;
using SetelaServerV3._1.Application.Features.AssignmentSubmissionFeature.Commands.DeleteAssignmentSubmissionCommand;
using SetelaServerV3._1.Application.Features.AssignmentSubmissionFeature.Commands.UpdateAssignmentSubmissionCommand;
using SetelaServerV3._1.Application.Features.AssignmentSubmissionFeature.DTO;
using SetelaServerV3._1.Application.Features.AssignmentSubmissionFeature.Queries.GetAssignmentSubmissionByIdQuery;
using SetelaServerV3._1.Application.Features.ResourceFeature.Commands.CreateResourceCommand;
using SetelaServerV3._1.Domain.Enums;
using SetelaServerV3._1.Shared.Common.Interfaces;
using SetelaServerV3._1.Shared.Utilities;
using System.Security.Claims;

namespace SetelaServerV3._1.Application.Features.AssignmentSubmissionFeature
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssignmentSubmissionController(IMediator _mediator, IFileStorage _storageService) : ControllerBase
    {
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<AssignmentSubmissionDTO>> GetById(int id)
        {
            var response = await _mediator.Send(new GetAssignmentSubmissionByIdQuery { AssignmentSubmissionId = id });
            return response.ToActionResult();
        }

        [Authorize]
        [HttpPost]
        [RequestSizeLimit(50 * 1024 * 1024)]
        public async Task<ActionResult<AssignmentSubmissionDTO>> Create([FromForm] CreateAssignmentSubmissionRequestDTO request)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var filesVerified = await _storageService.VerifyMultipleForSubmission(request.Files);
            if (!filesVerified.Success) return BadRequest(filesVerified.Error);

            var response = await _mediator.Send(new CreateAssignmentSubmissionCommand
            {
                UserId = int.Parse(userId),
                AssignmentSubmission = request,
                BaseUrl = $"{Request.Scheme}://{Request.Host}",
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

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<object>> Delete(int id)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var response = await _mediator.Send(new DeleteAssignmentSubmissionCommand
            {
                UserId = int.Parse(userId),
                AssignmentSubmissionId = id,
            });

            return response.ToActionResult();
        }
    }
}
