using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Infrastructure.Data;

namespace SetelaServerV3._1.Shared.Common.Services
{
    public class MaxDisplayOrder(AppDbContext _db)
    {
        public async Task<int> GetNext(int courseId, CancellationToken ct)
        {
            var topicTask = _db.TopicSeparators.Where(x => x.CourseId == courseId).MaxAsync(x => (int?)x.DisplayOrder, ct);
            var moduleTask = _db.Modules.Where(x => x.CourseId == courseId).MaxAsync(x => (int?)x.DisplayOrder, ct);
            var resourceTask = _db.Resources.Where(x => x.CourseId == courseId).MaxAsync(x => (int?)x.DisplayOrder, ct);

            await Task.WhenAll(topicTask, moduleTask, resourceTask);

            int max = new[] {
                topicTask.Result ?? 0,
                moduleTask.Result ?? 0,
                resourceTask.Result ?? 0
            }.Max();

            return max + 1;
        }
    }
}
