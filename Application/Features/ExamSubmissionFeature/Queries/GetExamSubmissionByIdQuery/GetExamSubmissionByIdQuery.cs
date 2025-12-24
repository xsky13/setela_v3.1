using MediatR;
using SetelaServerV3._1.Application.Features.ExamSubmissionFeature.DTO;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.ExamSubmissionFeature.Queries.GetExamSubmissionByIdQuery
{
    public class GetExamSubmissionByIdQuery : IRequest<Result<ExamSubmissionDTO>>
    {
        public int ExamSubmissionId { get; set; }
    }
}
