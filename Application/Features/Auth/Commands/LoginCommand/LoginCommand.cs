using MediatR;
using SetelaServerV3._1.Application.Features.Auth.DTO;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.Auth.Commands.LoginCommand
{
    public class LoginCommand : IRequest<Result<LoginResponse>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
