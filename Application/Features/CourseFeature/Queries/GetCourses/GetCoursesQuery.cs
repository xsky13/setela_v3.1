using MediatR;
using SetelaServerV3._1.Domain.Entities;

namespace SetelaServerV3._1.Application.Features.CourseFeature.Queries.GetCourses
{
    public class GetCoursesQuery : IRequest<Course[]>
    {
    }
}
