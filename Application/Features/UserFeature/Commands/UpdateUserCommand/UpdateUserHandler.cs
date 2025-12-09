using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Application.Features.UserFeature.DTO;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Policies;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.UserFeature.Commands.UpdateUserCommand
{
    public class UpdateUserHandler(AppDbContext _db, IMapper _mapper, IPermissionHandler _userPermissions) : IRequestHandler<UpdateUserCommand, Result<UserDTO>>
    {
        public async Task<Result<UserDTO>> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            if (!_userPermissions.CanEditUser(command.CurrentUserId, command.UserId, command.UserRoles))
                return Result<UserDTO>.Fail("No puede editar este usuario", 403);

            var user = await _db.SysUsers.FirstOrDefaultAsync(user => user.Id == command.UserId, cancellationToken);
            if (user == null) return Result<UserDTO>.Fail("El usuario no existe");

            var userWithEmailExists = await _db.SysUsers.AnyAsync(user => user.Email == command.Email && user.Id != command.UserId, cancellationToken);
            if (userWithEmailExists) return Result<UserDTO>.Fail("Ya hay un usuario con este email");

            user.Name = command.Name;
            user.Email = command.Email;

            await _db.SaveChangesAsync(cancellationToken);

            return Result<UserDTO>.Ok(_mapper.Map<UserDTO>(user));
        }
    }
}
