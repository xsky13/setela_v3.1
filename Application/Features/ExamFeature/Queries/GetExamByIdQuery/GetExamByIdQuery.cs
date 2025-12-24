using MediatR;
using SetelaServerV3._1.Application.Features.ExamFeature.DTO;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.ExamFeature.Queries.GetExamByIdQuery
{
    public class GetExamByIdQuery : IRequest<Result<ExamDTO>>
    {
        public int ExamId { get; set; }
    }
}
