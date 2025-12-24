using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Infrastructure.Data;
using System.Threading.Tasks;

namespace SetelaServerV3._1.Shared.Common.Services
{
    public class MaxDisplayOrder(AppDbContext _db)
    {
        public async Task<int> GetNext(int courseId, CancellationToken ct)
        {
            var topicTask = _db.TopicSeparators.Where(x => x.CourseId == courseId).MaxAsync(x => (int?)x.DisplayOrder, ct);
            var moduleTask = _db.Modules.Where(x => x.CourseId == courseId).MaxAsync(x => (int?)x.DisplayOrder, ct);
            var resourceTask = _db.Resources.Where(x => x.CourseId == courseId).MaxAsync(x => (int?)x.DisplayOrder, ct);
            var assignmentTask = _db.Assignments.Where(x => x.CourseId == courseId).MaxAsync(x => (int?)x.DisplayOrder, ct);
            var examTask = _db.Exams.Where(x => x.CourseId == courseId).MaxAsync(x => (int?)x.DisplayOrder, ct);

            await Task.WhenAll(topicTask, moduleTask, resourceTask);

            int max = new[] {
                topicTask.Result ?? 0,
                moduleTask.Result ?? 0,
                resourceTask.Result ?? 0,
                assignmentTask.Result ?? 0,
                examTask.Result ?? 0,
            }.Max();

            return max + 1;
        }
    }
}

//Task.Run(async () => {
//    using var db = _contextFactory.CreateDbContext();
//    return await db.TopicSeparators.Where(x => x.CourseId == courseId).MaxAsync(x => (int?)x.DisplayOrder, ct);
//}),
//                Task.Run(async () => {
//                    using var db = _contextFactory.CreateDbContext();
//                    return await db.Modules.Where(x => x.CourseId == courseId).MaxAsync(x => (int?)x.DisplayOrder, ct);
//                }),
//                Task.Run(async () => {
//                    using var db = _contextFactory.CreateDbContext();
//                    return await db.Resources.Where(x => x.CourseId == courseId).MaxAsync(x => (int?)x.DisplayOrder, ct);
//                }),
//                Task.Run(async () => {
//                    using var db = _contextFactory.CreateDbContext();
//                    return await db.Assignments.Where(x => x.CourseId == courseId).MaxAsync(x => (int?)x.DisplayOrder, ct);
//                }),
//            };

//var results = await Task.WhenAll(tasks);
//return (results.Max() ?? 0) + 1;