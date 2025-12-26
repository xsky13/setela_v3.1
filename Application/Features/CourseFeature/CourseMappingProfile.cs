using AutoMapper;
using SetelaServerV3._1.Application.Features.CourseFeature.DTO;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Domain.Enums;
using SetelaServerV3._1.Infrastructure.Data;

namespace SetelaServerV3._1.Application.Features.CourseFeature
{
    public class CourseMappingProfile : Profile
    {
        public CourseMappingProfile()
        {
            CreateMap<TopicSeparator, TopicDTO>();
            CreateMap<SysUser, UserForCourseDTO>();
            CreateMap<Enrollment, EnrollmentDTO>();
            CreateMap<Course, CourseDTO>()
                .ForMember(dest => dest.Resources, opt => opt.Ignore());

            CreateMap<Course, CourseDetailedDTO>()
                .ForMember(dest => dest.Students, opt => opt.MapFrom(src => src.Enrollments.Count))
                .ForMember(dest => dest.ExamsToGrade, opt => opt.MapFrom<ExamsToGradeResolver>())
                .ForMember(dest => dest.AssignmentsToGrade, opt => opt.MapFrom<AssignmentToGradeResolver>());
        }
    }

    public class ExamsToGradeResolver(AppDbContext db) : IValueResolver<Course, object, int>
    {
        public int Resolve(Course source, object destination, int destMember, ResolutionContext context)
        {
            return db.ExamSubmissions
                .Count(es => es.Exam.CourseId == source.Id &&
                             !db.Grades.Any(g => g.ParentId == es.Id &&
                                                  g.ParentType == GradeParentType.ExamSubmission));
        }
    }

    public class AssignmentToGradeResolver(AppDbContext db) : IValueResolver<Course, object, int>
    {
        public int Resolve(Course source, object destination, int destMember, ResolutionContext context)
        {
            return db.AssignmentSubmissions
                .Count(es => es.Assignment.CourseId == source.Id &&
                             !db.Grades.Any(g => g.ParentId == es.Id &&
                                                  g.ParentType == GradeParentType.AssignmentSubmission));
        }
    }
}
