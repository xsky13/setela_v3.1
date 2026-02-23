using MediatR;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.UserProgressFeature.GetProgressCommand
{
    public class GetProgressHandler(AppDbContext _db) : IRequestHandler<GetProgressCommand, Result<int>>
    {
        public async Task<Result<int>> Handle(GetProgressCommand command, CancellationToken cancellationToken)
        {

            throw new NotImplementedException();
        }
    }
}
