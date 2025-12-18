using AutoMapper;
using SetelaServerV3._1.Application.Features.CourseFeature.DTO;
using SetelaServerV3._1.Domain.Entities;

namespace SetelaServerV3._1.Application.Features.CourseFeature
{
    public class CourseMappingProfile : Profile
    {
        public CourseMappingProfile()
        {
            CreateMap<TopicSeparator, TopicDTO>()
                .ForMember(dest => dest.Resources, opt => opt.Ignore());
            CreateMap<SysUser, UserForCourseDTO>();
            CreateMap<Enrollment, EnrollmentDTO>();
            CreateMap<Course, CourseDTO>()
                .ForMember(dest => dest.Resources, opt => opt.Ignore());
        }
    }
}
