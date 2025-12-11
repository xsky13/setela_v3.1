using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Application.Features.TopicSeparatorFeature.DTO;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.TopicSeparatorFeature.Commands.UpdateTopicSeparator
{
    public class UpdateTopicSeparatorHandler(AppDbContext _db, IMapper _mapper) : IRequestHandler<UpdateTopicSeparatorCommand, Result<TopicSeparatorDTO>>
    {
        public async Task<Result<TopicSeparatorDTO>> Handle(UpdateTopicSeparatorCommand command, CancellationToken cancellationToken)
        {
            var topicSeparator = await _db.TopicSeparators.FirstOrDefaultAsync(topicSeparator => topicSeparator.Id == command.Id, cancellationToken);
            if (topicSeparator == null) return Result<TopicSeparatorDTO>.Fail("El separador no existe");

            topicSeparator.Title = command.NewTitle;
            await _db.SaveChangesAsync(cancellationToken);

            return Result<TopicSeparatorDTO>.Ok(_mapper.Map<TopicSeparatorDTO>(topicSeparator));
        }
    }
}
