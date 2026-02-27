using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Application.Features.ExamFeature.DTO;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Common.Services;
using SetelaServerV3._1.Shared.Policies;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.ExamFeature.Commands.CreateExamCommand
{
    public class CreateExamHandler(AppDbContext _db, IPermissionHandler _userPermissions, IMapper _mapper, MaxDisplayOrder maxDisplayOrder) : IRequestHandler<CreateExamCommand, Result<ExamDTO>>
    {
        public async Task<Result<ExamDTO>> Handle(CreateExamCommand command, CancellationToken cancellationToken)
        {
            if (!await _userPermissions.CanEditCourse(command.UserId, command.Exam.CourseId))
                return Result<ExamDTO>.Fail("No tiene permisos para crear examenes.");

            var courseExists = await _db.Courses.AnyAsync(course => course.Id == command.Exam.CourseId, cancellationToken);
            if (!courseExists) return Result<ExamDTO>.Fail("El curso que especifico no existe.");

            var exam = new Exam
            { 
                Title = command.Exam.Title,
                Description = command.Exam.Description,
                StartTime = command.Exam.StartTime,
                EndTime = command.Exam.EndTime,
                MaxGrade = command.Exam.MaxGrade,
                Weight = command.Exam.Weight,
                Visible = true,
                Closed = true,
                CreationDate = DateTime.UtcNow,
                DisplayOrder = await maxDisplayOrder.GetNext(command.Exam.CourseId, cancellationToken),
                CourseId = command.Exam.CourseId
            };

            _db.Exams.Add(exam);
            await _db.SaveChangesAsync(cancellationToken);

            return Result<ExamDTO>.Ok(_mapper.Map<ExamDTO>(exam));
        }
    }
}
