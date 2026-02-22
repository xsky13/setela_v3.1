using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Domain.Enums;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Common.Interfaces;
using SetelaServerV3._1.Shared.Policies;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.ResourceFeature.Commands.CreateMultipleResourcesCommand
{
    public class CreateMultipleResourcesHandler(AppDbContext _db, IPermissionHandler _userPermissions, IFileStorage _storageService) : IRequestHandler<CreateMultipleResourcesCommand, Result<List<Resource>>>
    {
        public async Task<Result<List<Resource>>> Handle(CreateMultipleResourcesCommand command, CancellationToken cancellationToken)
        {
            if (!Enum.TryParse(command.Request.ParentType, true, out ResourceParentType parentResourceType))
                return Result<List<Resource>>.Fail("Incorrect resource parent type");

            if (!await ParentExistsAsync(parentResourceType, command.Request.ParentId, cancellationToken))
                return Result<List<Resource>>.Fail($"La entidad no existe", 404);

            // check if parent has more than 10 resources
            var resourceAmount = await _db.Resources
                .Where(r => r.ParentId == command.Request.ParentId)
                .CountAsync(cancellationToken);

            if ((resourceAmount + command.Request.Files.Count) > 7)
                return Result<List<Resource>>.Fail($"No puede haber mas de 7 archivos por entrega.");

            bool canEdit = await _userPermissions.CanModifyResource(parentResourceType, command.UserId, command.Request.CourseId);
            if (!canEdit) return Result<List<Resource>>.Fail("No tiene permisos para crear recursos", 403);

            List<string> fileUrls = [];
            List<Resource> resources = [];

            try
            {
                foreach (var file in command.Request.Files)
                {
                    var fileSaveResponse = await _storageService.SaveFile(file, command.UserId);
                    if (!fileSaveResponse.Success) throw new Exception(fileSaveResponse.Error);

                    fileUrls.Add(fileSaveResponse.Value!);

                    var resource = new Resource
                    {
                        Url = $"{command.BaseUrl}/cdn/{command.UserId}/{fileSaveResponse.Value}",
                        LinkText = fileSaveResponse.Value,
                        ResourceType = ResourceType.Document,
                        ParentType = parentResourceType,
                        ParentId = command.Request.ParentId,
                        CreationDate = DateTime.UtcNow,
                        SysUserId = command.UserId,
                        CourseId = command.Request.CourseId,
                        DisplayOrder = 0,
                        Download = true
                    };
                    resources.Add(resource);
                }

                _db.Resources.AddRange(resources);
                await _db.SaveChangesAsync(cancellationToken);

                return Result<List<Resource>>.Ok(resources);
            } catch (Exception e)
            {
                foreach (var filename in fileUrls)
                    await _storageService.DeleteFile(filename, command.UserId);
                return Result<List<Resource>>.Fail(e.Message);
            }
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
