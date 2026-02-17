using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Application.Features.AssignmentSubmissionFeature.DTO;
using SetelaServerV3._1.Application.Features.ResourceFeature.Commands.CreateResourceCommand;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Domain.Enums;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Common.Interfaces;
using SetelaServerV3._1.Shared.Common.Services;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.AssignmentSubmissionFeature.Commands.CreateAssignmentSubmissionCommand
{
    public class CreateAssignmentSubmissionHandler(AppDbContext _db, IMapper _mapper, IFileStorage _storageService) : IRequestHandler<CreateAssignmentSubmissionCommand, Result<AssignmentSubmissionDTO>>
    {
        public async Task<Result<AssignmentSubmissionDTO>> Handle(CreateAssignmentSubmissionCommand command, CancellationToken cancellationToken)
        {
            var uploadedResources = new List<Resource>();
            using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var assignment = await _db.Assignments
                .Where(a => a.Id == command.AssignmentSubmission.AssignmentId)
                .Select(a => new { a.Closed })
                .FirstOrDefaultAsync(cancellationToken);
                if (assignment == null) return Result<AssignmentSubmissionDTO>.Fail("El trabajo practico no existe.");

                if (assignment.Closed)
                    return Result<AssignmentSubmissionDTO>.Fail("El trabajo practico esta cerrado");

                //var dueDate = assignment.DueDate.Kind == DateTimeKind.Unspecified
                //    ? DateTime.SpecifyKind(assignment.DueDate, DateTimeKind.Utc)
                //    : assignment.DueDate.ToUniversalTime();

                //if (DateTime.UtcNow > dueDate)
                //    return Result<AssignmentSubmissionDTO>.Fail("La fecha límite ha pasado.");

                bool tareaExiste = await _db.AssignmentSubmissions
                    .AnyAsync(a => a.SysUserId == command.UserId &&
                    a.AssignmentId == command.AssignmentSubmission.AssignmentId, cancellationToken);
                if (tareaExiste)
                    return Result<AssignmentSubmissionDTO>.Fail("Ya has enviado este trabajo.");

                var assignmentSubmission = new AssignmentSubmission
                {
                    TextContent = command.AssignmentSubmission.TextContent,
                    CreationDate = DateTime.UtcNow,
                    LastUpdateDate = DateTime.UtcNow,
                    SysUserId = command.UserId,
                    AssignmentId = command.AssignmentSubmission.AssignmentId
                };

                _db.AssignmentSubmissions.Add(assignmentSubmission);
                await _db.SaveChangesAsync(cancellationToken);


                foreach (var file in command.AssignmentSubmission.Files)
                {
                    var uploadResult = await _storageService.SaveFile(file);
                    if (!uploadResult.Success) throw new Exception(uploadResult.Error);


                    var resource = new Resource
                    {
                        Url = command.BaseUrl + "/cdn/" + uploadResult.Value,
                        LinkText = "",
                        ResourceType = ResourceType.Document,
                        ParentType = ResourceParentType.AssignmentSubmission,
                        ParentId = assignmentSubmission.Id,
                        CreationDate = DateTime.UtcNow,
                        SysUserId = command.UserId,
                        CourseId = command.AssignmentSubmission.CourseId,
                        DisplayOrder = 0,
                        Download = true
                    };

                    uploadedResources.Add(resource);
                }

                if (uploadedResources.Any())
                {
                    _db.Resources.AddRange(uploadedResources);
                    await _db.SaveChangesAsync(cancellationToken);
                }
                await transaction.CommitAsync(cancellationToken);

                return Result<AssignmentSubmissionDTO>.Ok(_mapper.Map<AssignmentSubmissionDTO>(assignmentSubmission));
            }
            catch (Exception)
            {
                foreach (var res in uploadedResources)
                {
                    await _storageService.DeleteFile(res.Url);
                }
                await transaction.RollbackAsync(cancellationToken);
                return Result<AssignmentSubmissionDTO>.Fail("Error al procesar los archivos.");
            }
        }
    }
}
