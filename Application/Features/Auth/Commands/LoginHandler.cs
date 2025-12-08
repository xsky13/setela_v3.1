using MediatR;

namespace SetelaServerV3._1.Application.Features.Auth.Commands
{
    public class LoginHandler : IRequestHandler<LoginCommand, string>
    {
        public async Task<string> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            return "";
        }
    }
}
