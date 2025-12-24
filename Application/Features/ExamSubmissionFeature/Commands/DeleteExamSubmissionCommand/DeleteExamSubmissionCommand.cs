using MediatR;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.ExamSubmissionFeature.Commands.DeleteExamSubmissionCommand
{
    public class DeleteExamSubmissionCommand : IRequest<Result<object>>
    {
        public int UserId { get; set; }
        public int ExamSubmissionId { get; set; }
    }
}
