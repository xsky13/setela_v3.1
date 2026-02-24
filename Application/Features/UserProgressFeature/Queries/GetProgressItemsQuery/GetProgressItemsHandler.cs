using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Common.DTO;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.UserProgressFeature.Queries.GetProgressItemsQuery
{
    public class GetProgressItemsHandler(AppDbContext _db, IMapper _mapper) : IRequestHandler<GetProgressItemsQuery, Result<List<UserProgressDTO>>>
    {
        public async Task<Result<List<UserProgressDTO>>> Handle(GetProgressItemsQuery query, CancellationToken cancellationToken)
        {
            var items = await _db.UserProgress
                .Where(p => p.SysUserId == query.UserId && p.CourseId == query.CourseId)
                .ProjectTo<UserProgressDTO>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return Result<List<UserProgressDTO>>.Ok(items);
        }
    }
}
