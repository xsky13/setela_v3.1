using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SetelaServerV3._1.Application.Features.Auth.Commands.LoginCommand;
using SetelaServerV3._1.Application.Features.Auth.Commands.RegisterCommand;
using SetelaServerV3._1.Application.Features.Auth.DTO;
using SetelaServerV3._1.Application.Features.UserFeature.DTO;
using SetelaServerV3._1.Application.Features.UserFeature.Queries.GetUserByIdQuery;
using SetelaServerV3._1.Shared.Utilities;
using System.Security.Claims;

namespace SetelaServerV3._1.Application.Features.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IMediator _mediator) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<LoginResponse>> Register([FromBody] RegisterCommand registerCommand)
        {
            var response = await _mediator.Send(registerCommand);
            return response.ToActionResult();
        }

        [Authorize]
        [HttpGet("user")]
        public async Task<ActionResult<UserDTO>> GetUser()
        {
            var currentUser = HttpContext.User;
            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var response = await _mediator.Send(new GetUserByIdQuery { Id = int.Parse(userId) });
            return response.ToActionResult();
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginCommand loginCommand)
        {
            var response = await _mediator.Send(loginCommand);
            return response.ToActionResult();
        }
    }
}
