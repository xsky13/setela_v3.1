using MediatR;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.UserFeature.Commands.ChangePasswordCommand
{
    public class ChangePasswordHandler(AppDbContext _db) : IRequestHandler<ChangePasswordCommand, Result<object>>
    {
        public async Task<Result<object>> Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
        {
            var user = await _db.SysUsers.FindAsync([command.UserId], cancellationToken);
            if (user == null) return Result<object>.Fail("El usuario no existe");

            if (!BCrypt.Net.BCrypt.Verify(command.OldPassword, user.PasswordHash))
                return Result<object>.Fail("La contraseña anterior no es correcta");

            if (command.NewPassword != command.NewPasswordRepeat)
                return Result<object>.Fail("Las contraseñas no son iguales");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(command.NewPassword);

            await _db.SaveChangesAsync(cancellationToken);
            return Result<object>.Ok(new { Success = true });
        }
    }
}
