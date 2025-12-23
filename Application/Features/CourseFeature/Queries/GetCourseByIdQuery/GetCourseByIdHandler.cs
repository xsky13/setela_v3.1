using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Application.Features.CourseFeature.DTO;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Infrastructure.Extensions;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.CourseFeature.Queries.GetCourseByIdQuery
{
    public class GetCourseByIdHandler(AppDbContext _db, IMapper _mapper) : IRequestHandler<GetCourseByIdQuery, Result<CourseDTO>>
    {
        public async Task<Result<CourseDTO>> Handle(GetCourseByIdQuery query, CancellationToken cancellationToken)
        {
            var course = await _db.Courses
                .Where(c => c.Id == query.CourseId)
                .ProjectTo<CourseDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken)
                .LoadResources(_db, _mapper, Domain.Enums.ResourceParentType.Course, cancellationToken);

            if (course == null)
                return Result<CourseDTO>.Fail("Este curso no existe", 404);
            
            return Result<CourseDTO>.Ok(course);
        }
    }
}
