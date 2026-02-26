using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Common.DTO;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.UserFeature.Queries.GetProfessorsQuery
{
    public class GetProfessorsHandler(AppDbContext _db, IMapper _mapper) : IRequestHandler<GetProfessorsQuery, Result<List<UserSimpleDTO>>>
    {
        public async Task<Result<List<UserSimpleDTO>>> Handle(GetProfessorsQuery query, CancellationToken cancellationToken)
        {
            var users = await _db.SysUsers
                .Where(u => u.Roles.Contains(Domain.Enums.UserRoles.Admin) || u.Roles.Contains(Domain.Enums.UserRoles.Professor))
                .ProjectTo<UserSimpleDTO>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return Result<List<UserSimpleDTO>>.Ok(users);
        }
    }
}
