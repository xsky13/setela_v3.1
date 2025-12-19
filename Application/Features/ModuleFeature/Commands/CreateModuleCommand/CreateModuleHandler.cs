using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Common.DTO;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.ModuleFeature.Commands.CreateModuleCommand
{
    public class CreateModuleHandler(AppDbContext _db, IMapper _mapper) : IRequestHandler<CreateModuleCommand, Result<ModuleSimpleDTO>>
    {
        public async Task<Result<ModuleSimpleDTO>> Handle(CreateModuleCommand command, CancellationToken cancellationToken)
        {
            var courseExists = await _db.Courses.AnyAsync(c => c.Id == command.CourseId, cancellationToken);
            if (!courseExists) return Result<ModuleSimpleDTO>.Fail("El curso no existe");
            var module = new Module
            {
                Title = command.Title,
                TextContent = command.TextContent,
                CreationDate = DateTime.UtcNow,
                CourseId = command.CourseId
            };

            _db.Modules.Add(module);
            await _db.SaveChangesAsync(cancellationToken);

            return Result<ModuleSimpleDTO>.Ok(_mapper.Map<ModuleSimpleDTO>(module));
        }
    }
}
