using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Application.Features.CourseFeature.DTO;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.CourseFeature.Queries.GetCourseByIdQuery
{
    public class GetCourseByIdHandler(AppDbContext _db, IMapper mapper) : IRequestHandler<GetCourseByIdQuery, Result<CourseDTO>>
    {
        public async Task<Result<CourseDTO>> Handle(GetCourseByIdQuery query, CancellationToken cancellationToken)
        {
            var course = await _db.Courses.ProjectTo<CourseDTO>(mapper.ConfigurationProvider).FirstOrDefaultAsync(course => course.Id == query.CourseId, cancellationToken);
            if (course == null)
                return Result<CourseDTO>.Fail("Este curso no existe", 404);

            return Result<CourseDTO>.Ok(course);
        }
    }
}
