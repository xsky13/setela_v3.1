using MediatR;

namespace SetelaServerV3._1.Application.Features.Auth.Commands
{
    public class LoginCommand : IRequest<string>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
