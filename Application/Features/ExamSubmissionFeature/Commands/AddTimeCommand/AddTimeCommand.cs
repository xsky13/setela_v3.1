using MediatR;
using SetelaServerV3._1.Application.Features.ExamSubmissionFeature.DTO;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.ExamSubmissionFeature.Commands.AddTimeCommand
{
    public class AddTimeCommand : IRequest<Result<ExamSubmissionDTO>>
    {
        public int ExamSubmissionId { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
    }
}
