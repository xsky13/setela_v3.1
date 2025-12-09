using MediatR;
using SetelaServerV3._1.Application.Features.CourseFeature.DTO;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.CourseFeature.Commands.DisenrollStudentCommand
{
    public class DisenrollStudentCommand : IRequest<Result<CourseDTO>>
    {
        public int CurrentUserId { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
    }
}
