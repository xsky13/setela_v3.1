using MediatR;
using SetelaServerV3._1.Application.Features.ExamSubmissionFeature.DTO;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.ExamSubmissionFeature.Commands.FinishExamCommand
{
    public class FinishExamCommand : IRequest<Result<ExamSubmissionDTO>>
    {
        public int UserId { get; set; }
        public int ExamSubmissionId { get; set; }
        public string? TextContent { get; set; }
        public int CourseId { get; set; }
        public string BaseUrl { get; set; }
    }
}
