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
            if (!await _userPermissions.CanEditCourse(command.UserId, command.CourseId))
                return Result<object>.Fail("No tiene permisos para editar este curso");

            var topicOrderMap = command.ReorderItems.TopicSeparators.ToDictionary(x => x.Id, x => x.NewOrder);
            var topicSeparators = await _db.TopicSeparators
                .Where(ts => topicOrderMap.Keys.Contains(ts.Id) && ts.CourseId == command.CourseId)
                .ToListAsync(cancellationToken);

            foreach (var topicSeparator in topicSeparators)
                topicSeparator.DisplayOrder = topicOrderMap[topicSeparator.Id];


            var moduleOrderMap = command.ReorderItems.Modules.ToDictionary(x => x.Id, x => x.NewOrder);
            var modules = await _db.Modules
                .Where(module => moduleOrderMap.Keys.Contains(module.Id) && module.CourseId == command.CourseId)
                .ToListAsync(cancellationToken);

            foreach (var module in modules)
                module.DisplayOrder = moduleOrderMap[module.Id];

            await _db.SaveChangesAsync(cancellationToken);

            return Result<object>.Ok(new { Success = true });
        }
    }
}
