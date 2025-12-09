using AutoMapper;
using SetelaServerV3._1.Application.Features.CourseFeature.DTO;
using SetelaServerV3._1.Domain.Entities;

namespace SetelaServerV3._1.Application.Features.CourseFeature
{
    public class CourseMappingProfile : Profile
    {
        public CourseMappingProfile()
        {
            CreateMap<SysUser, UserForCourseDTO>();
            CreateMap<Enrollment, EnrollmentDTO>();
                //.ForMember(dest => dest.SysUser, opt => opt.MapFrom(src => src.SysUser));
            CreateMap<Course, CourseDTO>();
        }
    }
}
