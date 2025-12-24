using AutoMapper;
using SetelaServerV3._1.Application.Features.ExamFeature.DTO;
using SetelaServerV3._1.Domain.Entities;

namespace SetelaServerV3._1.Application.Features.ExamFeature
{
    public class ExamMapingProfile : Profile
    {
        public ExamMapingProfile()
        {
            CreateMap<Exam, ExamDTO>();
        }
    }
}
