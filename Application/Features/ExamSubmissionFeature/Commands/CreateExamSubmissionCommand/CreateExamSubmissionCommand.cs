using MediatR;
using SetelaServerV3._1.Application.Features.ExamSubmissionFeature.DTO;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.ExamSubmissionFeature.Commands.CreateExamSubmissionCommand
{
    public class CreateExamSubmissionCommand : IRequest<Result<ExamSubmissionDTO>>
    {
        public int UserId { get; set; }
        public CreateExamSubmissionRequestDTO ExamSubmission { get; set; }
    }
}
