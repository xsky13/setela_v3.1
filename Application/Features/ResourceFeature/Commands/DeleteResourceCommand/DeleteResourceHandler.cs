using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Domain.Enums;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Common.Interfaces;
using SetelaServerV3._1.Shared.Policies;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.ResourceFeature.Commands.DeleteResourceCommand
{
    public class DeleteResourceHandler(AppDbContext _db, IPermissionHandler _userPermissions, IFileStorage _storageService) : IRequestHandler<DeleteResourceCommand, Result<object>>
    {
        public async Task<Result<object>> Handle(DeleteResourceCommand command, CancellationToken cancellationToken)
        {
            var resource = await _db.Resources.FindAsync([command.ResourceId], cancellationToken);
            if (resource == null) return Result<object>.Fail("El recurso especificado no existe");

            if (!await _userPermissions.CanModifyResource(resource.ParentType, command.UserId, resource.CourseId, resource.SysUserId))
                return Result<object>.Fail("No puede modificar este recurso", 403);

            if (resource.ParentType == ResourceParentType.AssignmentSubmission)
            {
                await _db.AssignmentSubmissions
                    .Where(a => a.Id == resource.ParentId)
                    .ExecuteUpdateAsync(s => s.SetProperty(b => b.LastUpdateDate, DateTime.UtcNow), cancellationToken);
            }

            // if this is an exam submission also update the last updated timestamp
            if (resource.ParentType == ResourceParentType.ExamSubmission)
            {
                await _db.ExamSubmissions
                    .Where(a => a.Id == resource.ParentId)
                    .ExecuteUpdateAsync(s => s.SetProperty(b => b.LastUdated, DateTime.UtcNow), cancellationToken);
            }

            if (resource.ResourceType == Domain.Enums.ResourceType.Document)
            {
                var result = await _storageService.DeleteFile(resource.Url, command.UserId);
                if (!result.Success) return Result<object>.Fail(result.Error);
            }

            _db.Resources.Remove(resource);
            await _db.SaveChangesAsync(cancellationToken);

            return Result<object>.Ok(new { Success = true });
        }
    }
}
