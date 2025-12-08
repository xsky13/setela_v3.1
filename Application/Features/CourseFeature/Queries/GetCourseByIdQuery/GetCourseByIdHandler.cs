using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.CourseFeature.Queries.GetCourseByIdQuery
{
    public class GetCourseByIdHandler(AppDbContext _db) : IRequestHandler<GetCourseByIdQuery, Result<Course>>
    {
        public async Task<Result<Course>> Handle(GetCourseByIdQuery query, CancellationToken cancellationToken)
        {
            var course = await _db.Courses.FirstOrDefaultAsync(course => course.Id == query.CourseId, cancellationToken);
            if (course == null)
                return Result<Course>.Fail("Este curso no existe", 404);

            return Result<Course>.Ok(course);
        }
    }
}
