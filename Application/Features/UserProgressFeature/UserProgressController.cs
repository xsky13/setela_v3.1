using MediatR;
using Microsoft.AspNetCore.Mvc;
using SetelaServerV3._1.Application.Features.UserProgressFeature.Commands.ToggleItemCommand;
using SetelaServerV3._1.Application.Features.UserProgressFeature.DTO;
using SetelaServerV3._1.Application.Features.UserProgressFeature.Queries.GetProgressQuery;
using SetelaServerV3._1.Shared.Common.DTO;
using SetelaServerV3._1.Shared.Utilities;
using System.Security.Claims;

namespace SetelaServerV3._1.Application.Features.UserProgressFeature
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProgressController(IMediator _mediator) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<int>> GetProgress(int id)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var response = await _mediator.Send(new GetProgressQuery
            {
                CourseId = id,
                UserId  = int.Parse(userId)
            });
            return response.ToActionResult();
        }

        [HttpPost]
        public async Task<ActionResult<UserProgressDTO>> ToggleProgress([FromBody] AddItemRequestDTO request)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var response = await _mediator.Send(new ToggleItemCommand
            {
                Request = request,
                UserId = int.Parse(userId)
            });
            return response.ToActionResult();
        }
    }
}
