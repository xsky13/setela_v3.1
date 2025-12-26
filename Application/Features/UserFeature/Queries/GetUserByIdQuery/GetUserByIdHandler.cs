using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Application.Features.UserFeature.DTO;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.UserFeature.Queries.GetUserByIdQuery
{
    public class GetUserByIdHandler(AppDbContext _db, IMapper _mapper) : IRequestHandler<GetUserByIdQuery, Result<UserDTO>>
    {
        public async Task<Result<UserDTO>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
        {
            var user = await _db.SysUsers
                .Include(u => u.ProfessorCourses)
                .Include(u => u.Enrollments)
                .FirstOrDefaultAsync(user => user.Id == query.Id, cancellationToken);

            if (user == null) return Result<UserDTO>.Fail("El usuario no existe", 404);

            return Result<UserDTO>.Ok(_mapper.Map<UserDTO>(user));
        }
    }
}
