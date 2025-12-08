using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.CourseFeature.Queries.GetCourses
{
    public class GetCoursesHandler(AppDbContext _db) : IRequestHandler<GetCoursesQuery, Course[]>
    {

        public async Task<Course[]> Handle(GetCoursesQuery query, CancellationToken cancellationToken)
        {
            List<Course> courses = await _db.Courses.ToListAsync(cancellationToken);
            return courses.ToArray();
        }

    }
}
