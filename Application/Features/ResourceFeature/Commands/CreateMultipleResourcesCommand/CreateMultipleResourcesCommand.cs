using MediatR;
using SetelaServerV3._1.Application.Features.ResourceFeature.DTO;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.ResourceFeature.Commands.CreateMultipleResourcesCommand
{
    public class CreateMultipleResourcesCommand : IRequest<Result<List<Resource>>>
    {
        public int UserId { get; set; }
        public string BaseUrl { get; set; }
        public CreateMultipleResourcesRequestDTO Request { get; set; }
    }
}
