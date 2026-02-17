using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Domain.Enums;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Common.Services;
using SetelaServerV3._1.Shared.Policies;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.ResourceFeature.Commands.CreateResourceCommand
{
    public class CreateResourceHandler(AppDbContext _db, IPermissionHandler _userPermissions, MaxDisplayOrder maxDisplayOrder) : IRequestHandler<CreateResourceCommand, Result<Resource>>
    {
        public async Task<Result<Resource>> Handle(CreateResourceCommand command, CancellationToken cancellationToken)
        {
            if (!Enum.TryParse(command.Type, true, out ResourceType resourceType))
                return Result<Resource>.Fail("Incorrect resource type");

            if (!Enum.TryParse(command.ParentType, true, out ResourceParentType parentResourceType))
                return Result<Resource>.Fail("Incorrect resource parent type");

            if (!await ParentExistsAsync(parentResourceType, command.ParentId, cancellationToken))
                return Result<Resource>.Fail("La entidad no existe", 404);

            if (!await _userPermissions.CanModifyResource(parentResourceType, command.UserId, command.CourseId))
                return Result<Resource>.Fail("No tiene permisos para crear recursos", 403);


            // if this is an assignment submission update the last updated timestamp
            if (parentResourceType == ResourceParentType.AssignmentSubmission)
            {
                await _db.AssignmentSubmissions
                    .Where(a => a.Id == command.ParentId)
                    .ExecuteUpdateAsync(s => s.SetProperty(b => b.LastUpdateDate, DateTime.UtcNow), cancellationToken);
            }

            // if this is an exam submission also update the last updated timestamp
            if (parentResourceType == ResourceParentType.ExamSubmission)
            {
                await _db.ExamSubmissions
                    .Where(a => a.Id == command.ParentId)
                    .ExecuteUpdateAsync(s => s.SetProperty(b => b.LastUdated, DateTime.UtcNow), cancellationToken);
            }

            string savedUrl;
            if (resourceType == ResourceType.Link)
            {
                savedUrl = command.Url;
            }
            else
            {
                savedUrl = $"{command.BaseUrl}/cdn/{command.Url}";
            }

            var newResource = new Resource
            { 
                Url = savedUrl,
                LinkText = command.LinkText,
                ResourceType = resourceType,
                ParentType = parentResourceType,
                ParentId = command.ParentId,
                CreationDate = DateTime.UtcNow,
                SysUserId = command.UserId,
                CourseId = command.CourseId,
                DisplayOrder = parentResourceType == ResourceParentType.Course ? await maxDisplayOrder.GetNext(command.CourseId, cancellationToken) : 0
            };

            _db.Resources.Add(newResource);
            await _db.SaveChangesAsync(cancellationToken);

            return Result<Resource>.Ok(newResource);
        }

        private async Task<bool> ParentExistsAsync(ResourceParentType parentType, int parentId, CancellationToken cancellationToken)
        {
            return parentType switch
            {
                ResourceParentType.Course => await _db.Courses.AnyAsync(c => c.Id == parentId, cancellationToken),
                ResourceParentType.TopicSeparator => await _db.TopicSeparators.AnyAsync(t => t.Id == parentId, cancellationToken),
                ResourceParentType.Module => await _db.Modules.AnyAsync(m => m.Id == parentId, cancellationToken),
                ResourceParentType.Assignment => await _db.Assignments.AnyAsync(m => m.Id == parentId, cancellationToken),
                ResourceParentType.AssignmentSubmission => await _db.AssignmentSubmissions.AnyAsync(m => m.Id == parentId, cancellationToken),
                ResourceParentType.Exam => await _db.Exams.AnyAsync(m => m.Id == parentId, cancellationToken),
                ResourceParentType.ExamSubmission => await _db.ExamSubmissions.AnyAsync(m => m.Id == parentId, cancellationToken),
                _ => false,
            };
        }
        
    
    }
}
