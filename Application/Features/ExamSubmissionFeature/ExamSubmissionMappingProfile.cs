using AutoMapper;
using SetelaServerV3._1.Application.Features.ExamSubmissionFeature.DTO;
using SetelaServerV3._1.Domain.Entities;

namespace SetelaServerV3._1.Application.Features.ExamSubmissionFeature
{
    public class ExamSubmissionMappingProfile : Profile
    {
        public ExamSubmissionMappingProfile()
        {
            CreateMap<ExamSubmission, ExamSubmissionDTO>();
        }
    }
}
