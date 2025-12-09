using MediatR;
using SetelaServerV3._1.Application.Features.CourseFeature.DTO;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.CourseFeature.Commands.RemoveProfessorCommand
{
    public class RemoveProfessorCommand : IRequest<Result<CourseDTO>>
    {
        public int CourseId { get; set; }
        public int UserId { get; set; }
    }
}
