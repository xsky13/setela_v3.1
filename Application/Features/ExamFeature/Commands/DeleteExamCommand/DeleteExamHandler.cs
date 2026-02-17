using AutoMapper;
using MediatR;
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
            var exam = await _db.Exams.FindAsync([command.ExamId], cancellationToken);
            if (exam == null) return Result<object>.Fail("El examen no existe.");

            if (!await _userPermissions.CanEditCourse(command.UserId, exam.CourseId))
                return Result<object>.Fail("No tiene permisos para editar examenes.");

            await _cleanupService.ClearParentResources(exam.Id, ResourceParentType.Exam, cancellationToken);

            _db.Exams.Remove(exam);
            await _db.SaveChangesAsync(cancellationToken);

            return Result<object>.Ok(new { Success = true });
        }
    }
}
