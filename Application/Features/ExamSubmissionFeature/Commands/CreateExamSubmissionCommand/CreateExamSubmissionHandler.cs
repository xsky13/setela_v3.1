using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Application.Features.ExamSubmissionFeature.DTO;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.ExamSubmissionFeature.Commands.CreateExamSubmissionCommand
{
    public class CreateExamSubmissionHandler(AppDbContext _db, IMapper _mapper) : IRequestHandler<CreateExamSubmissionCommand, Result<ExamSubmissionDTO>>
    {
        public async Task<Result<ExamSubmissionDTO>> Handle(CreateExamSubmissionCommand command, CancellationToken cancellationToken)
        {
            var examExists = await _db.Exams.AnyAsync(e => e.Id == command.ExamSubmission.ExamId, cancellationToken);
            if (!examExists) return Result<ExamSubmissionDTO>.Fail("El examen no existe");

            var studentSubmitted = await _db.ExamSubmissions
                .AnyAsync(e => e.ExamId == command.ExamSubmission.ExamId && e.SysUserId == command.UserId, cancellationToken);
            if (studentSubmitted)
                return Result<ExamSubmissionDTO>.Fail("Ya realizo una entrega para este examen.");

            var examSubmission = new ExamSubmission
            { 
                TextContent = command.ExamSubmission.TextContent ?? "",
                TurnInTime = DateTime.UtcNow,
                LastUdated = DateTime.UtcNow,
                SysUserId = command.UserId,
                ExamId = command.ExamSubmission.ExamId
            };

            _db.ExamSubmissions.Add(examSubmission);
            await _db.SaveChangesAsync(cancellationToken);

            return Result<ExamSubmissionDTO>.Ok(_mapper.Map<ExamSubmissionDTO>(examSubmission));
        }
    }
}
