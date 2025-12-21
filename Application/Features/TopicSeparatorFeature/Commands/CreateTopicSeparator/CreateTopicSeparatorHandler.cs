using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Application.Features.TopicSeparatorFeature.DTO;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Common.Services;
using SetelaServerV3._1.Shared.Policies;
using SetelaServerV3._1.Shared.Utilities;
using System.Security.Claims;

namespace SetelaServerV3._1.Application.Features.TopicSeparatorFeature.Commands.CreateTopicSeparator
{
    public class CreateTopicSeparatorHandler(AppDbContext _db, IMapper _mapper, IPermissionHandler _userPermissions, MaxDisplayOrder maxDisplayOrder) : IRequestHandler<CreateTopicSeparatorCommand, Result<TopicSeparatorDTO>>
    {
        public async Task<Result<TopicSeparatorDTO>> Handle(CreateTopicSeparatorCommand command, CancellationToken cancellationToken)
        {
            var canEdit = await _userPermissions.CanEditCourse(command.CurrentUserId, command.CourseId);
            if (!canEdit)
                return Result<TopicSeparatorDTO>.Fail("No tiene permisos para editar el curso");

            var courseExists = await _db.Courses.AnyAsync(course => course.Id == command.CourseId, cancellationToken);
            if (!courseExists) return Result<TopicSeparatorDTO>.Fail("El curso no existe");

            var separator = new TopicSeparator
            {
                Title = command.Title,
                CourseId = command.CourseId,
                DisplayOrder = await maxDisplayOrder.GetNext(command.CourseId, cancellationToken)
            };

            _db.TopicSeparators.Add(separator);
            await _db.SaveChangesAsync(cancellationToken);

            return Result<TopicSeparatorDTO>.Ok(_mapper.Map<TopicSeparatorDTO>(separator));
        }
    }
}
