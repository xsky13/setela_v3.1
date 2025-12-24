using MediatR;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.ExamSubmissionFeature.Commands.DeleteExamSubmissionCommand
{
    public class DeleteExamSubmissionHandler(AppDbContext _db) : IRequestHandler<DeleteExamSubmissionCommand, Result<object>>
    {
        public async Task<Result<object>> Handle(DeleteExamSubmissionCommand command, CancellationToken cancellationToken)
        {
            var submission = await _db.ExamSubmissions.FindAsync([command.ExamSubmissionId], cancellationToken);
            if (submission == null) return Result<object>.Fail("La entrega no existe");

            _db.ExamSubmissions.Remove(submission);
            await _db.SaveChangesAsync(cancellationToken);

            return Result<object>.Ok(new { Success = true });
        }
    }
}
