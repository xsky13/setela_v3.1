using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SetelaServerV3._1.Application.Features.GradeFeature.Commands.CreateGradeCommand;
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
    }
}
