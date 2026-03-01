using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Common.DTO;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.UserProgressFeature.Commands.ToggleItemCommand
{
    public class ToggleItemHandler(AppDbContext _db, IMapper _mapper) : IRequestHandler<ToggleItemCommand, Result<UserProgressDTO>>
    {
        public async Task<Result<UserProgressDTO>> Handle(ToggleItemCommand command, CancellationToken cancellationToken)
        {
            Console.WriteLine("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
            Console.WriteLine(command.Request.ParentType);
            Console.WriteLine(command.Request.CourseId);
            Console.WriteLine(command.Request.ParentId);
            var progressItem = await _db.UserProgress.FirstOrDefaultAsync(p =>
                p.ParentType == command.Request.ParentType &&
                p.ParentId == command.Request.ParentId &&
                p.SysUserId == command.UserId
            , cancellationToken);

            if (progressItem != null)
            {
                _db.UserProgress.Remove(progressItem);
                await _db.SaveChangesAsync(cancellationToken);
                return Result<UserProgressDTO>.Ok(null);
            }
            else
            {
                progressItem = new UserProgress
                {
                    ParentType = command.Request.ParentType,
                    ParentId = command.Request.ParentId,
                    SysUserId = command.UserId,
                    CourseId = command.Request.CourseId,
                };
                _db.UserProgress.Add(progressItem);
            }

            await _db.SaveChangesAsync(cancellationToken);
            return Result<UserProgressDTO>.Ok(_mapper.Map<UserProgressDTO>(progressItem));
        }
    }
}
