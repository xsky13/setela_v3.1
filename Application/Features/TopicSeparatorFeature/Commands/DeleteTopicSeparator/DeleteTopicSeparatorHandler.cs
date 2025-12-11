using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Application.Features.TopicSeparatorFeature.DTO;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.TopicSeparatorFeature.Commands.DeleteTopicSeparator
{
    public class DeleteTopicSeparatorHandler(AppDbContext _db, IMapper _mapper) : IRequestHandler<DeleteTopicSeparatorCommand, Result<object>>
    {
        public async Task<Result<object>> Handle(DeleteTopicSeparatorCommand command, CancellationToken cancellationToken)
        {
            var topicSeparator = await _db.TopicSeparators.FirstOrDefaultAsync(topicSeparator => topicSeparator.Id == command.Id, cancellationToken);
            if (topicSeparator == null) return Result<object>.Fail("El separador no existe");

            _db.TopicSeparators.Remove(topicSeparator);
            await _db.SaveChangesAsync(cancellationToken);

            return Result<object>.Ok(new { Success = true });
        }
    }
}
