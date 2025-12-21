using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SetelaServerV3._1.Application.Features.OrderingFeature.Commands.ReorderCourseCommand;
using SetelaServerV3._1.Application.Features.OrderingFeature.DTO;
using SetelaServerV3._1.Shared.Utilities;
using System.Security.Claims;

namespace SetelaServerV3._1.Application.Features.OrderingFeature
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderingController(IMediator _mediator) : ControllerBase
    {
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<object>> Reorder([FromBody] ReorderRequestDTO request)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var response = await _mediator.Send(new ReorderCourseCommand
            { 
                UserId = int.Parse(userId),
                ReorderItems = request
            });

            return response.ToActionResult();
        }
    }
}
