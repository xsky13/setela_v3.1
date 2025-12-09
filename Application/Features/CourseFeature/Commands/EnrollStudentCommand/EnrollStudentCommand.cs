using MediatR;
using SetelaServerV3._1.Application.Features.CourseFeature.DTO;
using SetelaServerV3._1.Domain.Enums;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.CourseFeature.Commands.EnrollStudentCommand
{
    public class EnrollStudentCommand : IRequest<Result<CourseDTO>>
    {
        public int CurrentUserId { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
    }
}
