using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Application.Features.UserFeature.DTO;
using SetelaServerV3._1.Infrastructure.Data;

namespace SetelaServerV3._1.Application.Features.UserFeature.Queries.GetUsersQuery
{
    public class GetUsersHandler(AppDbContext _db, IMapper _mapper) : IRequestHandler<GetUsersQuery, List<UserDTO>>
    {
        public async Task<List<UserDTO>> Handle(GetUsersQuery query, CancellationToken cancellationToken)
        {
            var users = await _db.SysUsers.ProjectTo<UserDTO>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
            return users;
        }
    }
}
