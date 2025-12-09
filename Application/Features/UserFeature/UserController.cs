using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SetelaServerV3._1.Application.Features.UserFeature.DTO;
using SetelaServerV3._1.Application.Features.UserFeature.Queries.GetUserByIdQuery;
using SetelaServerV3._1.Application.Features.UserFeature.Queries.GetUsersQuery;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.UserFeature
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IMediator _mediator) : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<UserDTO>>> GetUsers()
        {
            return Ok(await _mediator.Send(new GetUsersQuery()));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var response = await _mediator.Send(new GetUserByIdQuery { Id = id });
            return response.ToActionResult();
        }
    }
}
