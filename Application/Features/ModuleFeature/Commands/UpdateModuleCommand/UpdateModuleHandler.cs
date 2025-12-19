using AutoMapper;
using MediatR;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Common.DTO;
using SetelaServerV3._1.Shared.Policies;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.ModuleFeature.Commands.UpdateModuleCommand
{
    public class UpdateModuleHandler(AppDbContext _db, IMapper _mapper, IPermissionHandler _userPermissions) : IRequestHandler<UpdateModuleCommand, Result<ModuleSimpleDTO>>
    {
        public async Task<Result<ModuleSimpleDTO>> Handle(UpdateModuleCommand command, CancellationToken cancellationToken)
        {
            var module = await _db.Modules.FindAsync([command.ModuleId], cancellationToken);
            if (module == null) return Result<ModuleSimpleDTO>.Fail("El modulo no existe");

            if (!await _userPermissions.CanEditCourse(command.UserId, module.CourseId))
                return Result<ModuleSimpleDTO>.Fail("No tiene permisos para editar este curso", 403);

                if (command.Title != null) module.Title = command.Title;
            if (command.TextContent != null) module.TextContent = command.TextContent;
            if (command.Visible.HasValue) module.Visible = command.Visible.Value;

            await _db.SaveChangesAsync(cancellationToken);

            return Result<ModuleSimpleDTO>.Ok(_mapper.Map<ModuleSimpleDTO>(module));
        }
    }
}
