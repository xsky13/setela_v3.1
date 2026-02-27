using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Application.Features.UserFeature.DTO;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Common.Interfaces;
using SetelaServerV3._1.Shared.Policies;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.UserFeature.Commands.UpdateUserCommand
{
    public class UpdateUserHandler(AppDbContext _db, IMapper _mapper, IPermissionHandler _userPermissions, IFileStorage _storageService) : IRequestHandler<UpdateUserCommand, Result<UserDTO>>
    {
        public async Task<Result<UserDTO>> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            if (!_userPermissions.CanEditUser(command.CurrentUserId, command.UserId, command.UserRoles))
                return Result<UserDTO>.Fail("No puede editar este usuario", 403);

            var updatingUser = await _db.SysUsers
                .Where(u => u.Id == command.CurrentUserId)
                .Select(u => new { u.Roles })
                .FirstOrDefaultAsync(cancellationToken);
            if (updatingUser == null) return Result<UserDTO>.Fail("El usuario no existe");

            var user = await _db.SysUsers.FirstOrDefaultAsync(user => user.Id == command.UserId, cancellationToken);
            if (user == null) return Result<UserDTO>.Fail("El usuario no existe");

            var userWithEmailExists = await _db.SysUsers.AnyAsync(user => user.Email == command.Email && user.Id != command.UserId, cancellationToken);
            if (userWithEmailExists) return Result<UserDTO>.Fail("Ya hay un usuario con este email");

            if (command.NewPicture != null)
            {
                var filename = await _storageService.SaveFile(command.NewPicture, command.UserId);
                user.UserImageUrl = command.BaseUrl + "/cdn/" + command.UserId + '/' + filename.Value;
            }

            /**/

            if (updatingUser.Roles.Contains(Domain.Enums.UserRoles.Admin))
            {
                if (command.Password != null)
                {
                    if (command.Password.Length < 6) return Result<UserDTO>.Fail("La contraseña debe tener por lo menos 6 caracteres.");
                    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(command.Password);
                }
            }

            user.Name = command.Name;
            user.Email = command.Email;

            if (command.PhoneNumber != null) user.PhoneNumber = command.PhoneNumber;

            await _db.SaveChangesAsync(cancellationToken);

            return Result<UserDTO>.Ok(_mapper.Map<UserDTO>(user));
        }
    }
}
