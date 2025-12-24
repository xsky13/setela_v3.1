using MediatR;
using SetelaServerV3._1.Application.Features.ExamFeature.DTO;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.ExamFeature.Commands.UpdateExamCommand
{
    public class UpdateExamCommand : IRequest<Result<ExamDTO>>
    {
        public int UserId { get; set; }
        public int ExamId { get; set; }
        public UpdateExamRequestDTO Exam { get; set; }
    }
}
