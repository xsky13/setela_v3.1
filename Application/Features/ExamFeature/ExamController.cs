using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SetelaServerV3._1.Application.Features.ExamFeature.Commands.CreateExamCommand;
using SetelaServerV3._1.Application.Features.ExamFeature.Commands.DeleteExamCommand;
using SetelaServerV3._1.Application.Features.ExamFeature.Commands.UpdateExamCommand;
using SetelaServerV3._1.Application.Features.ExamFeature.DTO;
using SetelaServerV3._1.Application.Features.ExamFeature.Queries.GetExamByIdQuery;
using SetelaServerV3._1.Shared.Utilities;
using System.Security.Claims;

namespace SetelaServerV3._1.Application.Features.ExamFeature
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamController(IMediator _mediator) : ControllerBase
    {
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ExamDTO>> GetById(int id)
        {
            var response = await _mediator.Send(new GetExamByIdQuery { ExamId = id });
            return response.ToActionResult();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ExamDTO>> CreateExam([FromBody] CreateExamRequestDTO request)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var response = await _mediator.Send(new CreateExamCommand
            {
                UserId = int.Parse(userId),
                Exam = request
            });
            return response.ToActionResult();
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<ExamDTO>> Update([FromBody] UpdateExamRequestDTO request, int id)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var response = await _mediator.Send(new UpdateExamCommand
            {
                UserId = int.Parse(userId),
                ExamId = id,
                Exam = request
            });
            return response.ToActionResult();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<object>> DeleteExam(int id)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var response = await _mediator.Send(new DeleteExamCommand
            {
                UserId = int.Parse(userId),
                ExamId = id,
            });
            return response.ToActionResult();
        }


    }
}
