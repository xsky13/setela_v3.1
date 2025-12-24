using AutoMapper;
using MediatR;
using SetelaServerV3._1.Application.Features.ExamFeature.DTO;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Policies;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.ExamFeature.Commands.UpdateExamCommand
{
    public class UpdateExamHandler(AppDbContext _db, IPermissionHandler _userPermissions, IMapper _mapper) : IRequestHandler<UpdateExamCommand, Result<ExamDTO>>
    {
        public async Task<Result<ExamDTO>> Handle(UpdateExamCommand command, CancellationToken cancellationToken)
        {
            var exam = await _db.Exams.FindAsync([command.ExamId], cancellationToken);
            if (exam == null) return Result<ExamDTO>.Fail("El examen no existe.");

            if (!await _userPermissions.CanEditCourse(command.UserId, exam.CourseId))
                return Result<ExamDTO>.Fail("No tiene permisos para editar examenes.");

            if (command.Exam.Title != null) exam.Title = command.Exam.Title;
            if (command.Exam.Description != null) exam.Description = command.Exam.Description;
            if (command.Exam.MaxGrade.HasValue) exam.MaxGrade = command.Exam.MaxGrade.Value;
            if (command.Exam.Weight.HasValue) exam.Weight = command.Exam.Weight.Value;
            if (command.Exam.Visible.HasValue) exam.Visible = command.Exam.Visible.Value;
            if (command.Exam.StartTime.HasValue) exam.StartTime = command.Exam.StartTime.Value;
            if (command.Exam.EndTime.HasValue) exam.EndTime = command.Exam.EndTime.Value;

            await _db.SaveChangesAsync(cancellationToken);
            return Result<ExamDTO>.Ok(_mapper.Map<ExamDTO>(exam));
        }
    }
}
