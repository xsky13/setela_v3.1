using MediatR;
using SetelaServerV3._1.Application.Features.ExamFeature.DTO;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.ExamFeature.Commands.CreateExamCommand
{
    public class CreateExamCommand : IRequest<Result<ExamDTO>>
    {
        public int UserId { get; set; }
        public CreateExamRequestDTO Exam { get; set; }
    }
}
