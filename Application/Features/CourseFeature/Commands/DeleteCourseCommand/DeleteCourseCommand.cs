using MediatR;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.CourseFeature.Commands.DeleteCourseCommand
{
    public class DeleteCourseCommand : IRequest<Result<object>>
    {
        public int UserId { get; set; }
        public int CourseId { get; set; }
    }
}
