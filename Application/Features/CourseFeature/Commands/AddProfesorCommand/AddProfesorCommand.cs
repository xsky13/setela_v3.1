using MediatR;
using SetelaServerV3._1.Application.Features.CourseFeature.DTO;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.CourseFeature.Commands.AddProfesorCommand
{
    public class AddProfesorCommand : IRequest<Result<CourseDTO>>
    {
        public int CourseId { get; set; }
        public int UserId { get; set; }
    }
}
