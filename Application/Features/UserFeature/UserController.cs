using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SetelaServerV3._1.Application.Features.UserFeature.Commands.AddRoleToUserCommand;
using SetelaServerV3._1.Application.Features.UserFeature.Commands.RemoveRoleFromUserCommand;
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
        public async Task<ActionResult<List<UserListingDTO>>> GetUsers()
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            return Ok(await _mediator.Send(new GetUsersQuery
            {
                UserId = int.Parse(userId)
            }));
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
        public async Task<ActionResult<UserDTO>> UpdateUser([FromForm] UpdateUserRequestDTO request, int id)
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
                Email = request.Email,
                Password = request.Password,
                NewPicture = request.NewPicture,
                BaseUrl = $"{Request.Scheme}://{Request.Host}",
            });

            return response.ToActionResult();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("{id}/add_role")]
        public async Task<ActionResult<UserDTO>> AddRoleToUser([FromBody] UserRoleChangeRequestDTO request, int id)
        {
            var response = await _mediator.Send(new AddRoleToUserCommand { UserId = id, Role = request.Role });
            return response.ToActionResult();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("{id}/remove_role")]
        public async Task<ActionResult<UserDTO>> RemoveRoleFromUser([FromBody] UserRoleChangeRequestDTO request, int id)
        {
            var response = await _mediator.Send(new RemoveRoleFromUserCommand { UserId = id, Role = request.Role });
            return response.ToActionResult();
        }
    }
}
