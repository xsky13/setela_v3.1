using MediatR;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.GradeFeature.Commands.DeleteGradeCommand
{
    public class DeleteGradeCommand : IRequest<Result<object>>
    {
        public int GradeId { get; set; }
        public int UserId { get; set; }
    }
}
