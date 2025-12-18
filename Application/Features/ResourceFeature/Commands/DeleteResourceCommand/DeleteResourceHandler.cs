using MediatR;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Policies;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.ResourceFeature.Commands.DeleteResourceCommand
{
    public class DeleteResourceHandler(AppDbContext _db, IPermissionHandler _userPermissions) : IRequestHandler<DeleteResourceCommand, Result<object>>
    {
        public async Task<Result<object>> Handle(DeleteResourceCommand command, CancellationToken cancellationToken)
        {
            var resource = await _db.Resources.FindAsync([command.ResourceId], cancellationToken);
            if (resource == null) return Result<object>.Fail("El recurso especificado no existe");

            if (!await _userPermissions.CanModifyResource(resource.ParentType, command.UserId, resource.CourseId, resource.OwnerId))
                return Result<object>.Fail("No puede modificar este recurso", 403);

            _db.Resources.Remove(resource);
            await _db.SaveChangesAsync(cancellationToken);

            return Result<object>.Ok(new { Success = true });
        }
    }
}
