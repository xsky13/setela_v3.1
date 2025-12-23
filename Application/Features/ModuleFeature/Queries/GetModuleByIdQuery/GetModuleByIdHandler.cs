using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Application.Features.ModuleFeature.DTO;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Infrastructure.Extensions;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.ModuleFeature.Queries.GetModuleByIdQuery
{
    public class GetModuleByIdHandler(AppDbContext _db, IMapper _mapper) : IRequestHandler<GetModuleByIdQuery, Result<ModuleDTO>>
    {
        public async Task<Result<ModuleDTO>> Handle(GetModuleByIdQuery query, CancellationToken cancellationToken)
        {
            var module = await _db.Modules
                .Where(module => module.Id == query.ModuleId)
                .ProjectTo<ModuleDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken)
                .LoadResources(_db, _mapper, Domain.Enums.ResourceParentType.Module, cancellationToken);

            if (module == null) return Result<ModuleDTO>.Fail("El modulo no existe", 404);

            return Result<ModuleDTO>.Ok(module);
        }
    }
}
