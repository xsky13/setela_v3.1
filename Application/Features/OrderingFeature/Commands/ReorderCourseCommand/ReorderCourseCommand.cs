using MediatR;
using SetelaServerV3._1.Application.Features.OrderingFeature.DTO;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.OrderingFeature.Commands.ReorderCourseCommand
{
    public class ReorderCourseCommand : IRequest<Result<object>>
    {
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public ReorderRequestDTO ReorderItems { get; set; }
    }
}
