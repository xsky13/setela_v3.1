using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SetelaServerV3._1.Application.Features.ExamSubmissionFeature.Commands.CreateExamSubmissionCommand;
using SetelaServerV3._1.Application.Features.ExamSubmissionFeature.Commands.DeleteExamSubmissionCommand;
using SetelaServerV3._1.Application.Features.ExamSubmissionFeature.Commands.FinishExamCommand;
using SetelaServerV3._1.Application.Features.ExamSubmissionFeature.DTO;
using SetelaServerV3._1.Application.Features.ExamSubmissionFeature.Queries.GetExamSubmissionByIdQuery;
using SetelaServerV3._1.Shared.Utilities;
using System.Security.Claims;

namespace SetelaServerV3._1.Application.Features.ExamSubmissionFeature
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamSubmissionController(IMediator _mediator) : ControllerBase
    {
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ExamSubmissionDTO>> GetExamSubmissionById(int id)
        {
            var response = await _mediator.Send(new GetExamSubmissionByIdQuery
            {
                ExamSubmissionId = id
            });
            return response.ToActionResult();
        }


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

        [Authorize]
        [HttpPost("{id}/finish")]
        public async Task<ActionResult<ExamSubmissionDTO>> FinishExam(int id)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var response = await _mediator.Send(new FinishExamCommand
            {
                UserId = int.Parse(userId),
                ExamSubmissionId = id
            });
            return response.ToActionResult();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<object>> DeleteExamSubmission(int id)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var response = await _mediator.Send(new DeleteExamSubmissionCommand
            {
                UserId = int.Parse(userId),
                ExamSubmissionId = id
            });
            return response.ToActionResult();
        }
    }
}
