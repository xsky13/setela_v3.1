using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Domain.Enums;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Common.Interfaces;
using SetelaServerV3._1.Shared.Policies;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.CourseFeature.Commands.HardDeleteCourseCommand
{
    public class HardDeleteCourseHandler(AppDbContext _db, IPermissionHandler _userPermissions, IFileStorage _storageService) : IRequestHandler<HardDeleteCourseCommand, Result<object>>
    {
        public async Task<Result<object>> Handle(HardDeleteCourseCommand command, CancellationToken cancellationToken)
        {
            var canDelete = await _userPermissions.CanEditCourse(command.UserId, command.CourseId);
            if (!canDelete) return Result<object>.Fail("No tiene permisos para eliminar este curso", 403);

            var course = await _db.Courses
                .Include(c => c.Modules)
                .Include(c => c.Assignments)
                .Include(c => c.Exams)
                .FirstOrDefaultAsync(c => c.Id == command.CourseId, cancellationToken);

            if (course == null) return Result<object>.Fail("El curso no existe", 404);

            var assignmentSubIds = await _db.AssignmentSubmissions
                .Where(s => course.Assignments.Select(a => a.Id).Contains(s.AssignmentId))
                .Select(s => s.Id).ToListAsync(cancellationToken);

            var examSubIds = await _db.ExamSubmissions
                .Where(s => course.Exams.Select(e => e.Id).Contains(s.ExamId))
                .Select(s => s.Id).ToListAsync(cancellationToken);

            var moduleIds = course.Modules.Select(m => m.Id).ToList();
            var assignmentIds = course.Assignments.Select(m => m.Id).ToList();
            var examIds = course.Exams.Select(m => m.Id).ToList();

            var resourcesToRemove = await _db.Resources
                .Where(r => (r.ParentType == ResourceParentType.Course && r.ParentId == course.Id) ||
                            (r.ParentType == ResourceParentType.Module && moduleIds.Contains(r.ParentId)) ||
                            (r.ParentType == ResourceParentType.Assignment && assignmentIds.Contains(r.ParentId)) ||
                            (r.ParentType == ResourceParentType.AssignmentSubmission && assignmentSubIds.Contains(r.ParentId)) ||
                            (r.ParentType == ResourceParentType.Exam && examIds.Contains(r.ParentId)) ||
                            (r.ParentType == ResourceParentType.ExamSubmission && examSubIds.Contains(r.ParentId)))
                .ToListAsync(cancellationToken);

            var fileUrlsToDelete = resourcesToRemove
                .Where(r => r.ResourceType == ResourceType.Document)
                .Select(r => r.Url)
                .ToList();

            using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                _db.Resources.RemoveRange(resourcesToRemove);
                _db.Courses.Remove(course);

                await _db.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                foreach (var url in fileUrlsToDelete)
                    await _storageService.DeleteFile(url, command.UserId);

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
