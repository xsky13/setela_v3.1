using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Application.Features.UserFeature.DTO;
using SetelaServerV3._1.Infrastructure.Data;

namespace SetelaServerV3._1.Application.Features.UserFeature.Queries.GetUsersQuery
{
    public class GetUsersHandler(AppDbContext _db, IMapper _mapper) : IRequestHandler<GetUsersQuery, List<UserListingDTO>>
    {
        public async Task<List<UserListingDTO>> Handle(GetUsersQuery query, CancellationToken cancellationToken)
        {
            var users = await _db.SysUsers.Where(u => u.Id != query.UserId).ProjectTo<UserListingDTO>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
            return users;
        }
    }
}
