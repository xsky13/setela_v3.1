using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Application.Features.ExamSubmissionFeature.DTO;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Infrastructure.Extensions;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.ExamSubmissionFeature.Queries.GetExamSubmissionByIdQuery
{
    public class GetExamSubmissionByIdHandler(AppDbContext _db, IMapper _mapper) : IRequestHandler<GetExamSubmissionByIdQuery, Result<ExamSubmissionDTO>>
    {
        public async Task<Result<ExamSubmissionDTO>> Handle(GetExamSubmissionByIdQuery query, CancellationToken cancellationToken)
        {
            var submission = await _db.ExamSubmissions
                .Where(e => e.Id == query.ExamSubmissionId)
                .ProjectTo<ExamSubmissionDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken)
                .LoadResources(_db, _mapper, Domain.Enums.ResourceParentType.ExamSubmission, cancellationToken)
                .LoadGrades(_db, _mapper, Domain.Enums.GradeParentType.ExamSubmission, cancellationToken);

            Console.WriteLine("\ntest\n");
            Console.WriteLine(submission);
            Console.WriteLine("\n test");

            if (submission == null) return Result<ExamSubmissionDTO>.Fail("La entrega no existe", 404);

            return Result<ExamSubmissionDTO>.Ok(submission);
        }
    }
}
