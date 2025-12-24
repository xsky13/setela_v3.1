using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SetelaServerV3._1.Application.Features.GradeFeature.Commands.CreateGradeCommand;
using SetelaServerV3._1.Application.Features.GradeFeature.Commands.DeleteGradeCommand;
using SetelaServerV3._1.Application.Features.GradeFeature.Commands.UpdateGradeCommand;
using SetelaServerV3._1.Application.Features.GradeFeature.DTO;
using SetelaServerV3._1.Shared.Utilities;
using System.Security.Claims;

namespace SetelaServerV3._1.Application.Features.GradeFeature
{
    [ApiController]
    [Route("api/[controller]")]
    public class GradeController(IMediator _mediator) : ControllerBase
    {
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<GradeDTO>> CreateGrade([FromBody] CreateGradeRequestDTO request)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var response = await _mediator.Send(new CreateGradeCommand
            {
                UserId = int.Parse(userId),
                Grade = request
            });
            return response.ToActionResult();
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<GradeDTO>> UpdateGrade([FromBody] UpdateGradeRequestDTO request, int id)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var response = await _mediator.Send(new UpdateGradeCommand
            {
                UserId = int.Parse(userId),
                GradeId = id,
                Grade = request
            });
            return response.ToActionResult();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<object>> DeleteGrade(int id)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var response = await _mediator.Send(new DeleteGradeCommand
            {
                UserId = int.Parse(userId),
                GradeId = id,
            });
            return response.ToActionResult();
        }
    }
}
