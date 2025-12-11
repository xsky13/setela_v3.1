using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Application.Features.TopicSeparatorFeature.DTO;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Policies;
using SetelaServerV3._1.Shared.Utilities;
using System.Security.Claims;

namespace SetelaServerV3._1.Application.Features.TopicSeparatorFeature.Commands.CreateTopicSeparator
{
    public class CreateTopicSeparatorHandler(AppDbContext _db, IMapper _mapper, IPermissionHandler _userPermissions) : IRequestHandler<CreateTopicSeparatorCommand, Result<TopicSeparatorDTO>>
    {
        public async Task<Result<TopicSeparatorDTO>> Handle(CreateTopicSeparatorCommand command, CancellationToken cancellationToken)
        {
            var canEdit = await _userPermissions.CanEditCourse(command.CurrentUserId, command.CourseId);
            if (!canEdit)
                return Result<TopicSeparatorDTO>.Fail("No tiene permisos para editar el curso");

            var course = await _db.Courses
                .Include(course => course.TopicSeparators)
                .FirstOrDefaultAsync(course => course.Id == command.CourseId, cancellationToken);
            if (course == null) return Result<TopicSeparatorDTO>.Fail("El curso no existe");

            var separator = new TopicSeparator
            {
                Title = command.Title,
                CourseId = command.CourseId
            };

            course.TopicSeparators.Add(separator);
            await _db.SaveChangesAsync(cancellationToken);

            return Result<TopicSeparatorDTO>.Ok(_mapper.Map<TopicSeparatorDTO>(separator));
        }
    }
}
