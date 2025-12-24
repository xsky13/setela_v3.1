using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Policies;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.ExamSubmissionFeature.Commands.DeleteExamSubmissionCommand
{
    public class DeleteExamSubmissionHandler(AppDbContext _db, IPermissionHandler _userPermissions) : IRequestHandler<DeleteExamSubmissionCommand, Result<object>>
    {
        public async Task<Result<object>> Handle(DeleteExamSubmissionCommand command, CancellationToken cancellationToken)
        {
            var submission = await _db.ExamSubmissions
                .Where(e => e.Id == command.ExamSubmissionId)
                .Select(e => new { e.SysUserId, e.Exam.CourseId })
                .FirstOrDefaultAsync(cancellationToken);
            if (submission == null) return Result<object>.Fail("La entrega no existe");

            var canEditCourse = await _userPermissions.CanEditCourse(command.UserId, submission.CourseId);

            if (submission.SysUserId != command.UserId && !canEditCourse)
                return Result<object>.Fail("No puede borrar esta entrega", 403);

            await _db.ExamSubmissions
                .Where(e => e.Id == command.ExamSubmissionId)
                .ExecuteDeleteAsync(cancellationToken);

            return Result<object>.Ok(new { Success = true });
        }
    }
}
