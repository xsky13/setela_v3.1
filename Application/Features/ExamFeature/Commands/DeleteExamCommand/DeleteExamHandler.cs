using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Application.Features.ExamFeature.DTO;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Domain.Enums;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Common.Interfaces;
using SetelaServerV3._1.Shared.Policies;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.ExamFeature.Commands.DeleteExamCommand
{
    public class DeleteExamHandler(AppDbContext _db, IPermissionHandler _userPermissions, IResourceCleanupService _cleanupService) : IRequestHandler<DeleteExamCommand, Result<object>>
    {
        public async Task<Result<object>> Handle(DeleteExamCommand command, CancellationToken cancellationToken)
        {
            using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var exam = await _db.Exams.FindAsync([command.ExamId], cancellationToken);
                if (exam == null) return Result<object>.Fail("El examen no existe.");

                if (!await _userPermissions.CanEditCourse(command.UserId, exam.CourseId))
                    return Result<object>.Fail("No tiene permisos para editar examenes.");

                var examSubmissionsToRemove = await _db.ExamSubmissions
                    .Where(a => a.ExamId == exam.Id)
                    .ToListAsync(cancellationToken);

                _db.Exams.Remove(exam);
                await _db.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                foreach (var sub in examSubmissionsToRemove)
                    await _cleanupService.ClearParentResources(sub.Id, ResourceParentType.ExamSubmission, cancellationToken);

                await _cleanupService.ClearParentResources(exam.Id, ResourceParentType.Exam, cancellationToken);

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
