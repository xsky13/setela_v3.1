using MediatR;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.CourseFeature.Queries.GetCourseByIdQuery
{
    public class GetCourseByIdQuery : IRequest<Result<Course>>
    {
        public int CourseId { get; set; }
    }
}
