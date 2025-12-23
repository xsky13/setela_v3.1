using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Application.Features.AssignmentSubmissionFeature.DTO;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Infrastructure.Extensions;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.AssignmentSubmissionFeature.Queries.GetAssignmentSubmissionByIdQuery
{
    public class GetAssignmentSubmissionByIdHandler(AppDbContext _db, IMapper _mapper) : IRequestHandler<GetAssignmentSubmissionByIdQuery, Result<AssignmentSubmissionDTO>>
    {
        public async Task<Result<AssignmentSubmissionDTO>> Handle(GetAssignmentSubmissionByIdQuery query, CancellationToken cancellationToken)
        {
            var submission = await _db.AssignmentSubmissions
                .Where(a => a.Id == query.AssignmentSubmissionId)
                .ProjectTo<AssignmentSubmissionDTO>(_mapper.ConfigurationProvider)
                .LoadResources(_db, _mapper, Domain.Enums.ResourceParentType.AssignmentSubmission, cancellationToken);

            if (submission == null) return Result<AssignmentSubmissionDTO>.Fail("El trabajo practico no existe.");

            return Result<AssignmentSubmissionDTO>.Ok(submission);
        }
    }
}
