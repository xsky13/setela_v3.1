using MediatR;
using SetelaServerV3._1.Application.Features.GradeFeature.DTO;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.GradeFeature.Commands.CreateGradeCommand
{
    public class CreateGradeCommand : IRequest<Result<GradeDTO>>
    {
        public int UserId { get; set; }
        public CreateGradeRequestDTO Grade { get; set; }
    }
}
