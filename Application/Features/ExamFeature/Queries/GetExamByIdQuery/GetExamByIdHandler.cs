using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Application.Features.ExamFeature.DTO;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Infrastructure.Extensions;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.ExamFeature.Queries.GetExamByIdQuery
{
    public class GetExamByIdHandler(AppDbContext _db, IMapper _mapper) : IRequestHandler<GetExamByIdQuery, Result<ExamDTO>>
    {
        public async Task<Result<ExamDTO>> Handle(GetExamByIdQuery query, CancellationToken cancellationToken)
        {
            var exam = await _db.Exams
                .ProjectTo<ExamDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(e => e.Id == query.ExamId, cancellationToken)
                .LoadResources(_db, _mapper, Domain.Enums.ResourceParentType.Exam, cancellationToken);

            if (exam == null)
                return Result<ExamDTO>.Fail("El examen no existe.", 404);

            return Result<ExamDTO>.Ok(exam);
        }
    }
}
