using MediatR;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.ExamFeature.Commands.DeleteExamCommand
{
    public class DeleteExamCommand : IRequest<Result<object>>
    {
        public int UserId { get; set; }
        public int ExamId { get; set; }
    }
}
