using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Application.Features.UserFeature.DTO;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.UserFeature.Commands.AddRoleToUserCommand
{
    public class AddRoleToUserHandler(AppDbContext _db, IMapper _mapper) : IRequestHandler<AddRoleToUserCommand, Result<UserDTO>>
    {
        public async Task<Result<UserDTO>> Handle(AddRoleToUserCommand command, CancellationToken cancellationToken)
        {
            var userToUpdate = await _db.SysUsers.FirstOrDefaultAsync(user => user.Id == command.UserId, cancellationToken);
            if (userToUpdate == null) return Result<UserDTO>.Fail("El usuario no existe");

            if (userToUpdate.Roles.Contains(command.Role)) return Result<UserDTO>.Fail("El usuario ya tiene este rol");

            userToUpdate.Roles.Add(command.Role);
            await _db.SaveChangesAsync(cancellationToken);

            return Result<UserDTO>.Ok(_mapper.Map<UserDTO>(userToUpdate));
        }
    }
}
