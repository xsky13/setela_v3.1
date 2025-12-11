using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Application.Features.TopicSeparatorFeature.DTO;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Policies;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.TopicSeparatorFeature.Commands.DeleteTopicSeparator
{
    public class DeleteTopicSeparatorHandler(AppDbContext _db, IPermissionHandler _userPermissions) : IRequestHandler<DeleteTopicSeparatorCommand, Result<object>>
    {
        public async Task<Result<object>> Handle(DeleteTopicSeparatorCommand command, CancellationToken cancellationToken)
        {

            var topicSeparator = await _db.TopicSeparators.FirstOrDefaultAsync(topicSeparator => topicSeparator.Id == command.Id, cancellationToken);
            if (topicSeparator == null) return Result<object>.Fail("El separador no existe");
            
            // get the permissions after the topic separator to get the course
            var canEdit = await _userPermissions.CanEditCourse(command.CurrentUserId, topicSeparator.CourseId);
            if (!canEdit)
                return Result<object>.Fail("No tiene permisos para editar el curso");

            _db.TopicSeparators.Remove(topicSeparator);
            await _db.SaveChangesAsync(cancellationToken);

            return Result<object>.Ok(new { Success = true });
        }
    }
}
