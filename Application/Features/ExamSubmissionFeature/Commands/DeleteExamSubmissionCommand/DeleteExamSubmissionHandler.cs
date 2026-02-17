using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Domain.Enums;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Common.Interfaces;
using SetelaServerV3._1.Shared.Policies;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.ExamSubmissionFeature.Commands.DeleteExamSubmissionCommand
{
    public class DeleteExamSubmissionHandler(AppDbContext _db, IPermissionHandler _userPermissions, IResourceCleanupService _cleanupService) : IRequestHandler<DeleteExamSubmissionCommand, Result<object>>
    {
        public async Task<Result<object>> Handle(DeleteExamSubmissionCommand command, CancellationToken cancellationToken)
        {
            using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var submission = await _db.ExamSubmissions
                    .Where(e => e.Id == command.ExamSubmissionId)
                    .Select(e => new { e.SysUserId, e.Exam.CourseId, e.Id })
                    .FirstOrDefaultAsync(cancellationToken);
                if (submission == null) return Result<object>.Fail("La entrega no existe");

                var canEditCourse = await _userPermissions.CanEditCourse(command.UserId, submission.CourseId);

                if (submission.SysUserId != command.UserId && !canEditCourse)
                    return Result<object>.Fail("No puede borrar esta entrega", 403);

                await _db.ExamSubmissions
                    .Where(e => e.Id == command.ExamSubmissionId)
                    .ExecuteDeleteAsync(cancellationToken);

                await _cleanupService.ClearParentResources(submission.Id, ResourceParentType.ExamSubmission, cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return Result<object>.Ok(new { Success = true });
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                return Result<object>.Fail("Error al eliminar la entrega y sus archivos.");
            }
        }
    }
}
