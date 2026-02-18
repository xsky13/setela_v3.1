using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Common.DTO;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.CourseFeature.Queries.GetCourses
{
    public class GetCoursesHandler(AppDbContext _db, IMapper _mapper) : IRequestHandler<GetCoursesQuery, List<CourseListingDTO>>
    {

        public async Task<List<CourseListingDTO>> Handle(GetCoursesQuery query, CancellationToken cancellationToken)
        {

            var user = await _db.SysUsers
                .Where(u => u.Id == query.UserId)
                .Select(u => new { u.Roles })
                .FirstOrDefaultAsync(cancellationToken);

            List<CourseListingDTO> courses = [];
            if (user.Roles.Contains(Domain.Enums.UserRoles.Admin))
            {
                courses = await _db.Courses
                    .ProjectTo<CourseListingDTO>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
            } else
            {
                courses = await _db.Courses
                    .Where(c => c.IsActive)
                    .ProjectTo<CourseListingDTO>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
            }
            return courses;
        }

    }
}
