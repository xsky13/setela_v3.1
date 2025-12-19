using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Common.DTO;
using SetelaServerV3._1.Shared.Policies;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.ModuleFeature.Commands.CreateModuleCommand
{
    public class CreateModuleHandler(AppDbContext _db, IMapper _mapper, IPermissionHandler _userPermissions) : IRequestHandler<CreateModuleCommand, Result<ModuleSimpleDTO>>
    {
        public async Task<Result<ModuleSimpleDTO>> Handle(CreateModuleCommand command, CancellationToken cancellationToken)
        {
            if (!await _userPermissions.CanEditCourse(command.UserId, command.CourseId))
                return Result<ModuleSimpleDTO>.Fail("No tiene permisos para editar este curso", 403);

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
