using AutoMapper;
using SetelaServerV3._1.Application.Features.TopicSeparatorFeature.DTO;
using SetelaServerV3._1.Domain.Entities;

namespace SetelaServerV3._1.Application.Features.TopicSeparatorFeature
{
    public class TopicSeparatorMappingProfile : Profile
    {
        public TopicSeparatorMappingProfile()
        {
            CreateMap<Course, CourseDTO>();
            CreateMap<TopicSeparator, TopicSeparatorDTO>();
        }
    }
}
