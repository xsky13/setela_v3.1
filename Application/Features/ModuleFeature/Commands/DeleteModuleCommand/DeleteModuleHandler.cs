using MediatR;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Domain.Enums;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Common.Interfaces;
using SetelaServerV3._1.Shared.Policies;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.ModuleFeature.Commands.DeleteModuleCommand
{
    public class DeleteModuleHandler(AppDbContext _db, IPermissionHandler _userPermissions, IResourceCleanupService _cleanupService) : IRequestHandler<DeleteModuleCommand, Result<object>>
    {
        public async Task<Result<object>> Handle(DeleteModuleCommand command, CancellationToken cancellationToken)
        {
            var module = await _db.Modules.FindAsync([command.ModuleId], cancellationToken);
            if (module == null) return Result<object>.Fail("El modulo no existe", 404);

            if (!await _userPermissions.CanEditCourse(command.UserId, module.CourseId))
                return Result<object>.Fail("No tiene permisos para eliminar este modulo", 403);

            await _cleanupService.ClearParentResources(module.Id, ResourceParentType.Module, cancellationToken);

            _db.Modules.Remove(module);
            await _db.SaveChangesAsync(cancellationToken);

            return Result<object>.Ok(new { Success = true });
        }
    }
}
