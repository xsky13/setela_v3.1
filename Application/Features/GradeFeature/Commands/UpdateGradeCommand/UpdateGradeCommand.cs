using MediatR;
using SetelaServerV3._1.Application.Features.GradeFeature.DTO;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.GradeFeature.Commands.UpdateGradeCommand
{
    public class UpdateGradeCommand : IRequest<Result<GradeDTO>>
    {
        public int UserId { get; set; }
        public int GradeId { get; set; }
        public UpdateGradeRequestDTO Grade { get; set; }
    }
}
