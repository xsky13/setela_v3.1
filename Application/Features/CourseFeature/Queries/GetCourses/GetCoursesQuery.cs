using MediatR;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.CourseFeature.Queries.GetCourses
{
    public class GetCoursesQuery : IRequest<Course[]>
    {
    }
}
