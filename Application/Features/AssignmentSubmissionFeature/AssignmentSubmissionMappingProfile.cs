using AutoMapper;
using SetelaServerV3._1.Application.Features.AssignmentSubmissionFeature.DTO;
using SetelaServerV3._1.Domain.Entities;

namespace SetelaServerV3._1.Application.Features.AssignmentSubmissionFeature
{
    public class AssignmentSubmissionMappingProfile : Profile
    {
        public AssignmentSubmissionMappingProfile()
        {
            CreateMap<AssignmentSubmission, AssignmentSubmissionDTO>();
        }
    }
}
