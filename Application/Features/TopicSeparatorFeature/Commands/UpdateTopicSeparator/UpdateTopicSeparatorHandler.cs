using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Application.Features.TopicSeparatorFeature.DTO;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Policies;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.TopicSeparatorFeature.Commands.UpdateTopicSeparator
{
    public class UpdateTopicSeparatorHandler(AppDbContext _db, IMapper _mapper, IPermissionHandler _userPermissions) : IRequestHandler<UpdateTopicSeparatorCommand, Result<TopicSeparatorDTO>>
    {
        public async Task<Result<TopicSeparatorDTO>> Handle(UpdateTopicSeparatorCommand command, CancellationToken cancellationToken)
        {

            var topicSeparator = await _db.TopicSeparators.FirstOrDefaultAsync(topicSeparator => topicSeparator.Id == command.Id, cancellationToken);
            if (topicSeparator == null) return Result<TopicSeparatorDTO>.Fail("El separador no existe");

            // putting it below so i can get the courseid
            var canEdit = await _userPermissions.CanEditCourse(command.CurrentUserId, topicSeparator.CourseId);
            if (!canEdit)
                return Result<TopicSeparatorDTO>.Fail("No tiene permisos para editar el curso");

            topicSeparator.Title = command.NewTitle;
            await _db.SaveChangesAsync(cancellationToken);

            return Result<TopicSeparatorDTO>.Ok(_mapper.Map<TopicSeparatorDTO>(topicSeparator));
        }
    }
}
