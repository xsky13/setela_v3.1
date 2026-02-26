using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Policies;
using SetelaServerV3._1.Shared.Utilities;
using System.Linq;

namespace SetelaServerV3._1.Application.Features.OrderingFeature.Commands.ReorderCourseCommand
{
    public class ReorderCourseHandler(AppDbContext _db, IPermissionHandler _userPermissions) : IRequestHandler<ReorderCourseCommand, Result<object>>
    {
        public async Task<Result<object>> Handle(ReorderCourseCommand command, CancellationToken cancellationToken)
        {
            if (!await _userPermissions.CanEditCourse(command.UserId, command.ReorderItems.CourseId))
                return Result<object>.Fail("No tiene permisos para editar este curso");

            Console.WriteLine("Request: \n");

            if (command.ReorderItems.TopicSeparators != null && command.ReorderItems.TopicSeparators.Count != 0)
            {
                var topicOrderMap = command.ReorderItems.TopicSeparators.ToDictionary(x => x.Id, x => x.NewOrder);
                var topicSeparators = await _db.TopicSeparators
                    .Where(ts => topicOrderMap.Keys.Contains(ts.Id) && ts.CourseId == command.ReorderItems.CourseId)
                    .ToListAsync(cancellationToken);

                foreach (var topicSeparator in topicSeparators)
                    topicSeparator.DisplayOrder = topicOrderMap[topicSeparator.Id];
            }

            if (command.ReorderItems.Modules != null && command.ReorderItems.Modules.Count != 0)
            {

                var moduleOrderMap = command.ReorderItems.Modules.ToDictionary(x => x.Id, x => x.NewOrder);
                var modules = await _db.Modules
                    .Where(module => moduleOrderMap.Keys.Contains(module.Id) && module.CourseId == command.ReorderItems.CourseId)
                    .ToListAsync(cancellationToken);

                foreach (var module in modules)
                    module.DisplayOrder = moduleOrderMap[module.Id];
            }

            if (command.ReorderItems.Assignments != null && command.ReorderItems.Assignments.Count != 0)
            {

                var assignmentOrderMap = command.ReorderItems.Assignments.ToDictionary(x => x.Id, x => x.NewOrder);
                var assignments = await _db.Assignments
                    .Where(assignment => assignmentOrderMap.Keys.Contains(assignment.Id) && assignment.CourseId == command.ReorderItems.CourseId)
                    .ToListAsync(cancellationToken);

                foreach (var assignment in assignments)
                    assignment.DisplayOrder = assignmentOrderMap[assignment.Id];
            }

            if (command.ReorderItems.Exams != null && command.ReorderItems.Exams.Count != 0)
            {

                var examOrderMap = command.ReorderItems.Exams.ToDictionary(x => x.Id, x => x.NewOrder);
                var exams = await _db.Exams
                    .Where(exam => examOrderMap.Keys.Contains(exam.Id) && exam.CourseId == command.ReorderItems.CourseId)
                    .ToListAsync(cancellationToken);

                foreach (var exam in exams)
                    exam.DisplayOrder = examOrderMap[exam.Id];
            }

            if (command.ReorderItems.Resources != null && command.ReorderItems.Resources.Count != 0)
            {

                var resourceOrderMap = command.ReorderItems.Resources.ToDictionary(x => x.Id, x => x.NewOrder);
                var resources = await _db.Resources
                    .Where(resource => resourceOrderMap.Keys.Contains(resource.Id) && resource.CourseId == command.ReorderItems.CourseId)
                    .ToListAsync(cancellationToken);

                foreach (var resource in resources)
                    resource.DisplayOrder = resourceOrderMap[resource.Id];
            }

            await _db.SaveChangesAsync(cancellationToken);

            return Result<object>.Ok(new { Success = true });
        }
    }
}
