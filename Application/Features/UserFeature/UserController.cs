using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SetelaServerV3._1.Application.Features.UserFeature.Commands.UpdateUserCommand;
using SetelaServerV3._1.Application.Features.UserFeature.DTO;
using SetelaServerV3._1.Application.Features.UserFeature.Queries.GetUserByIdQuery;
using SetelaServerV3._1.Application.Features.UserFeature.Queries.GetUsersQuery;
using SetelaServerV3._1.Domain.Enums;
using SetelaServerV3._1.Shared.Utilities;
using System.Security.Claims;

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

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDTO>> UpdateUser([FromBody] UpdateUserRequestDTO request, int id)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            List<string> roleStrings = currentUser.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
            List<UserRoles> userRoleEnums = roleStrings
                .Select(roleString => Enum.TryParse<UserRoles>(roleString, true, out UserRoles result) ? (UserRoles?)result : null)
                .Where(role => role.HasValue)
                .Select(role => role!.Value)
                .ToList();

            var response = await _mediator.Send(new UpdateUserCommand
            {
                CurrentUserId = int.Parse(userId),
                UserId = id,
                UserRoles = userRoleEnums,
                Name = request.Name,
                Email = request.Email
            });

            return response.ToActionResult();
        }
    }
}
