using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SetelaServerV3._1.Application.Features.ExamSubmissionFeature.Commands.CreateExamSubmissionCommand;
using SetelaServerV3._1.Application.Features.ExamSubmissionFeature.DTO;
using SetelaServerV3._1.Shared.Utilities;
using System.Security.Claims;

namespace SetelaServerV3._1.Application.Features.ExamSubmissionFeature
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamSubmissionController(IMediator _mediator) : ControllerBase
    {
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ExamSubmissionDTO>> CreateExamSubmission([FromBody] CreateExamSubmissionRequestDTO request)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var response = await _mediator.Send(new CreateExamSubmissionCommand
            {
                UserId = int.Parse(userId),
                ExamSubmission = request
            });
            return response.ToActionResult();
        }
    }
}
